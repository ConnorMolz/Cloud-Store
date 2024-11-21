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