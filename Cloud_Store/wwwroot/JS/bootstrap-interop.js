window.initializeBootstrapDropdowns = () => {
    // Use querySelectorAll to find all dropdown toggle buttons
    const dropdownElementList = document.querySelectorAll('[data-bs-toggle="dropdown"]');

    // Initialize each dropdown
    dropdownElementList.forEach(dropdownToggleEl => {
        // Check if the dropdown is already initialized
        if (!dropdownToggleEl.dropdown) {
            new bootstrap.Dropdown(dropdownToggleEl);
        }
    });
};

window.downloadFile = (fileName, base64Data) => {
    // Create a Blob from the base64 data
    const byteCharacters = atob(base64Data);
    const byteNumbers = new Array(byteCharacters.length);

    for (let i = 0; i < byteCharacters.length; i++) {
        byteNumbers[i] = byteCharacters.charCodeAt(i);
    }

    const byteArray = new Uint8Array(byteNumbers);
    const blob = new Blob([byteArray], { type: 'application/octet-stream' });

    // Create a link element and trigger download
    const link = document.createElement('a');
    link.href = URL.createObjectURL(blob);
    link.download = fileName;
    document.body.appendChild(link);
    link.click();

    // Clean up
    document.body.removeChild(link);
    URL.revokeObjectURL(link.href);
};