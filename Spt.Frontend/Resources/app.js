window.scrollToTopSmooth = function() {
    window.scrollTo({
        top: 0,
        behavior: 'smooth'
    });
}

window.toggleScrollButton = function() {
    var scrollButton = document.getElementById('scrollTopBtn');
    if (!scrollButton) {
        return;
    }
    if (document.body.scrollTop > 20 || document.documentElement.scrollTop > 20) {
        scrollButton.style.display = 'flex';
    } else {
        scrollButton.style.display = 'none';
    }
};

window.onscroll = function() {
    window.toggleScrollButton();
};

window.toggleScrollButton();
