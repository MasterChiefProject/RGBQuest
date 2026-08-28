import test from "node:test";
import assert from "node:assert/strict";
import fs from "node:fs";
import path from "node:path";

const root = process.cwd();

function read(relativePath) {
  return fs.readFileSync(path.join(root, relativePath), "utf8");
}

test("WebGL HUD fallback does not depend on legacy font glyph rendering", () => {
  const fallback = read("Assets/Scripts/WebGLVisualFallbacks.cs");
  const hearts = read("Assets/Scripts/WebGLHealthHeartsGraphic.cs");
  const crosshair = read("Assets/Scripts/WebGLCrosshairGraphic.cs");

  assert.match(fallback, /#if UNITY_WEBGL && !UNITY_EDITOR/);
  assert.match(fallback, /gameObject\.name == "Health"/);
  assert.match(fallback, /gameObject\.name == "Crosshair"/);
  assert.match(fallback, /WebGLHealthHeartsGraphic/);
  assert.match(fallback, /WebGLCrosshairGraphic/);
  assert.match(fallback, /legacyText\.enabled = false/);
  assert.match(hearts, /Mathf\.Clamp\(Globals\.health, 0, MaxHealth\)/);
  assert.match(hearts, /class WebGLHealthHeartsGraphic/);
  assert.match(crosshair, /class WebGLCrosshairGraphic/);
  assert.match(crosshair, /AddQuad/);
});

test("WebGL Mobile renderer includes the decal feature required by Level 2 danger zones", () => {
  const renderer = read("Assets/Settings/Mobile_Renderer.asset");

  assert.match(renderer, /m_Name: DecalRendererFeature/);
  assert.match(renderer, /m_Active: 1/);
  assert.match(renderer, /technique: 0/);
  assert.match(
    renderer,
    /m_RendererFeatures:\s*\n\s*- \{fileID: -3058524210762481510\}/
  );
  assert.match(renderer, /m_RendererFeatureMap: 9a0092606ff08dd5/);
});

test("CI checks include the WebGL visual parity files", () => {
  const workflow = read(".github/workflows/repository.yml");

  for (const requiredPath of [
    "Assets/Scripts/WebGLVisualFallbacks.cs",
    "Assets/Scripts/WebGLHealthHeartsGraphic.cs",
    "Assets/Scripts/WebGLCrosshairGraphic.cs",
    "Assets/Settings/Mobile_Renderer.asset",
  ]) {
    assert.ok(workflow.includes(requiredPath));
  }
});
