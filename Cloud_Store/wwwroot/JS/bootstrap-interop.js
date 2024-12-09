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

// Requires PDF.js library to be included
window.renderPdf = function (containerId, base64Pdf) {
    const container = document.getElementById(containerId);
    if (!container) {
        console.error('Container not found');
        return;
    }

    // Convert base64 to Uint8Array
    const pdfData = atob(base64Pdf);
    const pdfBytes = new Uint8Array(pdfData.length);
    for (let i = 0; i < pdfData.length; i++) {
        pdfBytes[i] = pdfData.charCodeAt(i);
    }

    // Use PDF.js to render the document
    pdfjsLib.getDocument({ data: pdfBytes }).promise.then(function (pdf) {
        // Render first page
        pdf.getPage(1).then(function (page) {
            const scale = 1.5;
            const viewport = page.getViewport({ scale: scale });

            // Prepare canvas for rendering
            const canvas = document.createElement('canvas');
            const context = canvas.getContext('2d');
            canvas.height = viewport.height;
            canvas.width = viewport.width;

            // Render PDF page into canvas context
            const renderContext = {
                canvasContext: context,
                viewport: viewport
            };
            page.render(renderContext);

            // Clear previous content and add new canvas
            container.innerHTML = '';
            container.appendChild(canvas);
        });
    }).catch(function (error) {
        console.error('Error rendering PDF:', error);
        container.innerHTML = `<div class="alert alert-danger">Error rendering PDF: ${error.message}</div>`;
    });
};
