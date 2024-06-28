document.getElementById('spinButton').addEventListener('click', function () {
    var wheel = document.querySelector('.spin-wheel');
    var randomDegree = Math.floor(Math.random() * (Math.random() * 5000) + 360); // Ensures at least one full spin
    wheel.style.transition = 'transform 4s ease-out';
    wheel.style.transform = 'rotate(' + randomDegree + 'deg)';
});
