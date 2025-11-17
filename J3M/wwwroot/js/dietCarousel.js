(function () {
    document.addEventListener('DOMContentLoaded', () => {
        const slides = Array.from(document.querySelectorAll('.diet-slide'));
        const prevBtn = document.querySelector('.diet-prev');
        const nextBtn = document.querySelector('.diet-next');

        let index = 0;
        let autoplayId = null;

        function updateActive() {
            slides.forEach((slide, i) => {
                const link = slide.closest('.diet-link');
                const isActive = i === index;

                slide.classList.toggle('active', isActive);
                link.classList.toggle('active', isActive);
            });
        }


        function goTo(newIndex) {
            if (newIndex < 0) index = slides.length - 1;
            else if (newIndex >= slides.length) index = 0;
            else index = newIndex;

            updateActive(); // only change the active slide
        }

        function next() { goTo(index + 1); }
        function prev() { goTo(index - 1); }

        function startAutoplay() {
            stopAutoplay();
            autoplayId = setInterval(next, 3000);
        }

        function stopAutoplay() {
            if (autoplayId) clearInterval(autoplayId);
        }

        if (nextBtn) nextBtn.addEventListener("click", () => { next(); startAutoplay(); });
        if (prevBtn) prevBtn.addEventListener("click", () => { prev(); startAutoplay(); });

        // init
        updateActive();
        startAutoplay();
    });
})();
