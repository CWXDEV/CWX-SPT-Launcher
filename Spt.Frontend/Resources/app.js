window.startDragging = function (startX, startY) {

    function onMouseMove(event) {
        const offsetX = event.clientX - startX;
        const offsetY = event.clientY - startY;

        DotNet.invokeMethodAsync('CWX-SPT-Frontend', 'MoveWindow', offsetX, offsetY);
    }

    function onMouseUp() {
        document.removeEventListener('mousemove', onMouseMove);
        document.removeEventListener('mouseup', onMouseUp);
    }

    document.addEventListener('mousemove', onMouseMove);
    document.addEventListener('mouseup', onMouseUp);

};

window.scrollToTopSmooth = function() {
    window.scrollTo({
        top: 0,
        behavior: 'smooth'
    });
}

// Function to show/hide the button based on scroll position
window.toggleScrollButton = function() {
    var scrollButton = document.getElementById('scrollTopBtn');
    if (!scrollButton) {
        console.log('ScrollButton was null');
    }
    if (document.body.scrollTop > 20 || document.documentElement.scrollTop > 20) {
        scrollButton.style.display = 'flex';
    } else {
        scrollButton.style.display = 'none';
    }
};

// Attach scroll event listener
window.onscroll = function() {
    window.toggleScrollButton();
};

// Initial check (in case page is loaded already scrolled)
window.toggleScrollButton();
