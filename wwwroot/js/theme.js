// window.themeManager = {
//     setTheme: function (theme) {
//         const html = document.documentElement;

//         if (theme === "Auto") {
//             this.applySystemTheme();

//             return;
//         }

//         html.setAttribute("data-bs-theme", theme.toLowerCase());
//     },

//     applySystemTheme: function () {
//         const prefersDark = window.matchMedia(
//             "(prefers-color-scheme: dark)"
//         ).matches;

//         document.documentElement.setAttribute(
//             "data-bs-theme",
//             prefersDark ? "dark" : "light"
//         );
//     }
// };
window.themeManager = (() => {

    const storageKey = "theme";
    const mediaQuery = window.matchMedia("(prefers-color-scheme: dark)");

    let systemThemeListener = null;

    function applySystemTheme() {
        const theme = mediaQuery.matches ? "dark" : "light";

        document.documentElement.setAttribute("data-bs-theme", theme);
    }

    function setTheme(theme) {

        localStorage.setItem(storageKey, theme);

        // Rimuove eventuali listener precedenti
        if (systemThemeListener) {
            mediaQuery.removeEventListener("change", systemThemeListener);
            systemThemeListener = null;
        }

        if (theme === "Auto") {

            applySystemTheme();

            systemThemeListener = applySystemTheme;

            mediaQuery.addEventListener("change", systemThemeListener);

        }
        else {

            document.documentElement.setAttribute(
                "data-bs-theme",
                theme.toLowerCase()
            );

        }
    }

    function initialize(defaultTheme) {

        const savedTheme = localStorage.getItem(storageKey);

        const validThemes = ["Light", "Dark", "Auto"];

        const theme = validThemes.includes(savedTheme)
            ? savedTheme
            : defaultTheme;

        setTheme(theme);

        return theme;
    }

    return {
        setTheme,
        initialize
    };

})();