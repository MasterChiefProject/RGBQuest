import test from "node:test";
import assert from "node:assert/strict";
import fs from "node:fs";
import path from "node:path";

const root = process.cwd();

function read(relativePath) {
  return fs.readFileSync(path.join(root, relativePath), "utf8");
}

test("project remains on Unity 6000.0.47f1", () => {
  const version = read("ProjectSettings/ProjectVersion.txt");
  assert.match(version, /m_EditorVersion:\s*6000\.0\.47f1/);
});

test("existing Build Settings keep the six enabled production scenes", () => {
  const settings = read("ProjectSettings/EditorBuildSettings.asset");

  for (const scene of [
    "MainMenu",
    "Level1",
    "Level2",
    "Level3",
    "DeathMenu",
    "WinMenu",
  ]) {
    assert.match(
      settings,
      new RegExp(
        `enabled:\\s*1\\s*\\n\\s*path:\\s*Assets/Scenes/${scene}\\.unity`
      )
    );
  }
});

test("Main Menu serialized UnityEvent method names are preserved", () => {
  const menu = read("Assets/Scripts/MainMenu.cs");

  for (const method of [
    "ExitButtonHandle",
    "StartButtonHandle",
    "AboutButtonHandle",
    "BackButtonHandle",
  ]) {
    assert.match(menu, new RegExp(`public void ${method}\\(\\)`));
  }

  assert.match(menu, /public GameObject mainMenuPanel/);
  assert.match(menu, /public GameObject aboutPanel/);
});

test("pressure-plate serialized and trigger contract is preserved", () => {
  const plate = read("Assets/Scripts/PressurePlate.cs");

  assert.match(plate, /public string matchColor/);
  assert.match(plate, /public CubeGlow targetLamp/);
  assert.match(plate, /OnTriggerEnter\(Collider other\)/);
  assert.match(plate, /OnTriggerExit\(Collider other\)/);
  assert.match(plate, /animator\.SetBool\("isPressed"/);
  assert.match(plate, /targetLamp\.LightUp\(\)/);
  assert.match(plate, /targetLamp\.LightDown\(\)/);
});

test("health pickup keeps its UI, audio, and trigger contract", () => {
  const health = read("Assets/Scripts/HealthBox.cs");

  assert.match(health, /public TextMeshProUGUI displayText/);
  assert.match(health, /public AudioSource healthSound/);
  assert.match(health, /OnTriggerEnter\(Collider other\)/);
  assert.match(health, /StartCoroutine\(PlayAndDestroy\(\)\)/);
});

test("ammo pickup keeps its serialized UI and audio contract", () => {
  const ammo = read("Assets/Scripts/AmmoBox.cs");

  assert.match(ammo, /public TextMeshProUGUI textDisplay/);
  assert.match(ammo, /public int ammoInBox/);
  assert.match(ammo, /public Text ammoTextUI/);
  assert.match(ammo, /private AudioSource ammoSound/);
  assert.match(ammo, /OnTriggerEnter\(Collider other\)/);
});

test("portal door keeps its Animator and audio behavior", () => {
  const door = read("Assets/Scripts/PortalDoor.cs");

  assert.match(door, /public AudioClip openAudioClip/);
  assert.match(door, /private AudioSource audioSource/);
  assert.match(door, /private Animator animator/);
  assert.match(door, /animator\.SetBool\("isOpen"/);
  assert.match(door, /audioSource\.PlayOneShot\(openAudioClip\)/);
});

test("production build helper contains only the six release scenes", () => {
  const build = read("Assets/Editor/RGBQuestWebGLBuild.cs");
  const matches = [...build.matchAll(/Assets\/Scenes\/[^\"]+\.unity/g)]
    .map(match => match[0]);

  assert.deepEqual(
    [...new Set(matches)],
    [
      "Assets/Scenes/MainMenu.unity",
      "Assets/Scenes/Level1.unity",
      "Assets/Scenes/Level2.unity",
      "Assets/Scenes/Level3.unity",
      "Assets/Scenes/DeathMenu.unity",
      "Assets/Scenes/WinMenu.unity",
    ]
  );
});

test("production helper never changes rendering or quality configuration", () => {
  const build = read("Assets/Editor/RGBQuestWebGLBuild.cs");

  assert.doesNotMatch(build, /QualitySettings/);
  assert.doesNotMatch(build, /GraphicsSettings/);
  assert.doesNotMatch(build, /RenderPipeline/);
  assert.doesNotMatch(build, /Mobile_RPAsset/);
  assert.doesNotMatch(build, /PC_RPAsset/);
  assert.doesNotMatch(build, /SwitchActiveBuildTarget/);

  assert.match(
    build,
    /activeBuildTarget\s*!=\s*BuildTarget\.WebGL/
  );
});

test("production helper applies only safe deployment metadata", () => {
  const build = read("Assets/Editor/RGBQuestWebGLBuild.cs");

  assert.match(
    build,
    /PlayerSettings\.companyName\s*=\s*"MasterChiefProject"/
  );
  assert.match(
    build,
    /PlayerSettings\.productName\s*=\s*"RGBQuest"/
  );
  assert.match(
    build,
    /PlayerSettings\.bundleVersion\s*=\s*"1\.0\.0"/
  );
  assert.match(build, /WebGLCompressionFormat\.Gzip/);
  assert.match(build, /decompressionFallback\s*=\s*true/);
  assert.match(build, /dataCaching\s*=\s*true/);
});

test("production helper stages before publishing docs", () => {
  const build = read("Assets/Editor/RGBQuestWebGLBuild.cs");

  assert.match(build, /Builds\/RGBQuestWebGLStaging/);
  assert.match(build, /ReplacePublishedBuild/);
  assert.match(build, /\.nojekyll/);
});

test("README prominently documents trailer and verified controls", () => {
  const readme = read("README.md");

  assert.match(readme, /Trailer and complete stage guide/);
  assert.match(readme, /youtube\.com\/watch\?v=b8so2yYArQA/);
  assert.match(readme, /\| Move \| `WASD` or Arrow keys \|/);
  assert.match(readme, /\| Crouch \| `C` \|/);
  assert.match(readme, /\| Pick up \/ drop a physics object \| `E` \|/);
  assert.match(readme, /\| Throw a held object \| `T` \|/);
  assert.match(readme, /Hold `R` \+ Mouse/);
});

test("README documents the manual WebGL safety gate", () => {
  const readme = read("README.md");

  assert.match(readme, /Switch to WebGL manually/);
  assert.match(readme, /If materials become pink/);
  assert.match(readme, /before building/);
});

test("custom WebGL shell exposes source, guide, theme, and fullscreen", () => {
  const html = read("Assets/WebGLTemplates/RGBQuest/index.html");
  const shell = read("Assets/WebGLTemplates/RGBQuest/TemplateData/shell.js");

  assert.match(html, /github\.com\/MasterChiefProject\/RGBQuest/);
  assert.match(html, /youtube\.com\/watch\?v=b8so2yYArQA/);
  assert.match(html, /data-theme="dark"/);
  assert.match(html, /unity-fullscreen-button/);
  assert.match(html, /devicePixelRatio:\s*1/);
  assert.match(shell, /rgbquest-theme-v1/);
  assert.match(shell, /localStorage\.setItem/);
});

test("documented Unity package versions remain present", () => {
  const manifest = read("Packages/manifest.json");

  assert.match(manifest, /"com\.unity\.render-pipelines\.universal":\s*"17\.0\.4"/);
  assert.match(manifest, /"com\.unity\.inputsystem":\s*"1\.14\.0"/);
  assert.match(manifest, /"com\.unity\.ai\.navigation":\s*"2\.0\.8"/);
  assert.match(manifest, /"com\.unity\.cinemachine":\s*"3\.1\.4"/);
});
