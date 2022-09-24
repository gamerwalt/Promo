var element = document.querySelector("#kt_modal_create_campaign");
var previousButton = document.getElementById('previous');

var stepper = new KTStepper(element);
previousButton.style.display = 'block';

// Handle navigation click
stepper.on("kt.stepper.click", function (stepper) {
    stepper.goTo(stepper.getClickedStepIndex()); // go to clicked step
    previousButton.style.display = 'block';
});

// Handle next step
stepper.on("kt.stepper.next", function (stepper) {
    stepper.goNext(); // go next step
    previousButton.style.display = 'block';
});

// Handle previous step
stepper.on("kt.stepper.previous", function (stepper) {
    stepper.goPrevious(); // go previous step
    previousButton.style.display = 'block';
});
