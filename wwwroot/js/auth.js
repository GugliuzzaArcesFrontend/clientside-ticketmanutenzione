window.authLogin = async (loginDto) => {
    const response = await fetch("/auth/login", {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        },
        credentials: "same-origin",
        body: JSON.stringify(loginDto)
    });

    const data = await response.json().catch(() => null);

    return {
        success: response.ok,
        status: response.status,
        message: data?.message ?? ""
    };
};