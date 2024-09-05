var pdfjsLib = window['pdfjs-dist/build/pdf'];
pdfjsLib.GlobalWorkerOptions.workerSrc = 'https://cdnjs.cloudflare.com/ajax/libs/pdf.js/2.6.347/pdf.worker.min.js';
var scale = 1; // Escala de zoom
var resolution = 1; // Resolución de PDF

function LoadPdfFromUrl(url, containerId) {
    try {
        // Obtener el contenedor correspondiente según el ID del journal
        var pdf_container = document.getElementById("pdf_container_" + containerId);
        pdf_container.innerHTML = ""; // Limpiar el contenedor previo

        // Cargar el PDF
        pdfjsLib.getDocument(url).promise.then(function (pdfDoc) {
            // Renderizar todas las páginas del PDF
            for (var i = 1; i <= pdfDoc.numPages; i++) {
                RenderPage(pdfDoc, pdf_container, i);
            }
        });
    }
    catch (error) {
        console.error("Error:", error);
    }
}

function RenderPage(pdfDoc, pdf_container, num) {
    pdfDoc.getPage(num).then(function (page) {
        // Crear un nuevo elemento canvas
        var canvas = document.createElement('canvas');
        var ctx = canvas.getContext('2d');
        pdf_container.appendChild(canvas);

        // Establecer dimensiones del canvas
        var viewport = page.getViewport({ scale: scale });
        canvas.height = resolution * viewport.height;
        canvas.width = resolution * viewport.width;

        // Renderizar la página del PDF
        var renderContext = {
            canvasContext: ctx,
            viewport: viewport,
            transform: [resolution, 0, 0, resolution, 0, 0]
        };

        page.render(renderContext);
    });
}