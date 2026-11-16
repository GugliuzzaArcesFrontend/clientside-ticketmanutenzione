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

window.scrollElementToTop = (element) => {
    // console.log("Elemento ricevuto:", element);
    // console.log("Tipo:", typeof element);
    // console.log("Tag:", element?.tagName);
    // console.log("ScrollTop:", element?.scrollTop);
    // console.log("ScrollHeight:", element?.scrollHeight);
    // console.log("ClientHeight:", element?.clientHeight);
    
    if (!element || typeof element.scrollTo !== "function") {
        console.error("Elemento non valido o non scrollabile:", element);
        return;
    }
    if (!element || !element.tagName) {
        console.warn("ScrollTarget non ancora disponibile.");
        return;
    }
    element.scrollTo({
        top: 0,
        left: 0,
        behavior: 'smooth'
    });
};

window.isElementYScrollable = (element) => {

    // if (!element || !element.tagName)
    //     return false;

    // const overflowY = getComputedStyle(element).overflowY;

    // const allowsScrolling =
    //     overflowY === 'auto' ||
    //     overflowY === 'scroll';

    // return allowsScrolling
    //     && element.scrollHeight > element.clientHeight;
    if (!element || !element.tagName)
        return false;

    const style = getComputedStyle(element);

    console.table({
        tagName: element.tagName,
        overflowY: style.overflowY,
        height: element.clientHeight,
        scrollHeight: element.scrollHeight,
        isScrollable: element.scrollHeight > element.clientHeight
    });

    return element.scrollHeight > element.clientHeight;
};

window.isElementXScrollable = (element) => {
    if (!element || !element.tagName) return false;

    const overflowX = getComputedStyle(element).overflowX;

    const allowScrolling = overflowX === 'auto' || overflowX === 'scroll';

    return allowScrolling && element.scrollWidth > element.clientWidth;
};