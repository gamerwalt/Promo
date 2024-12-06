var currentRotation = 0;

document.getElementById("spinButton").addEventListener("click", function () {
    var spinButton = document.getElementById("spinButton");
    spinButton.disabled = true;
    var pie = document.querySelector(".pie");
    var randomDegree = Math.floor(Math.random() * 3600 + 360);
    currentRotation += randomDegree;
    pie.style.transition = "transform 4s ease-out";
    pie.style.transform = "rotate(" + currentRotation + "deg)";

    setTimeout(function () {
        document.querySelector(".container").style.display = "none";
        document.querySelector(".result-page").style.display = "flex";
        var prizeIndex = Math.floor(((currentRotation % 360) + 22.5) / 45) % 8;
        var prizes = [
            "Prize 1",
            "Prize 2",
            "Prize 3",
            "Prize 4",
            "Prize 5",
            "Prize 6",
            "Prize 7",
            "Prize 8",
        ];
        document.getElementById("prize").textContent = prizes[prizeIndex];
    }, 4000);
});
