(() => {
  "use strict";

  const key = "rgbquest-theme-v1";
  const root = document.documentElement;
  const toggle = document.getElementById("theme-toggle");

  function currentTheme() {
    try {
      return localStorage.getItem(key) === "light"
        ? "light"
        : "dark";
    } catch (_) {
      return "dark";
    }
  }

  function applyTheme(theme, persist = true) {
    const value = theme === "light" ? "light" : "dark";
    root.dataset.theme = value;

    if (toggle) {
      const isLight = value === "light";
      toggle.textContent = isLight ? "Dark mode" : "Light mode";
      toggle.setAttribute("aria-pressed", String(isLight));
    }

    if (persist) {
      try {
        localStorage.setItem(key, value);
      } catch (_) {}
    }
  }

  applyTheme(currentTheme(), false);

  if (toggle) {
    toggle.addEventListener("click", () => {
      applyTheme(
        root.dataset.theme === "light"
          ? "dark"
          : "light"
      );
    });
  }
})();
