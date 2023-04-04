const MAX_SELECTED_TILES = 3;
const submitBtn = document.querySelector('button[type="submit"]');
const resetBtn = document.getElementById('reset');

document.querySelectorAll('.tile').forEach(tile => {
    tile.addEventListener('click', () => {
        tile.classList.toggle('selected');
        updateSelectedTilesInput();

        const selectedTiles = document.querySelectorAll('.tile.selected');
        if (selectedTiles.length === MAX_SELECTED_TILES) {
            submitBtn.disabled = false;
        } else {
            submitBtn.disabled = true;
        }

        resetBtn.disabled = false;
    });
});

resetBtn.addEventListener('click', () => {
    document.querySelectorAll('.tile').forEach(tile => {
        tile.classList.remove('selected');
    });

    document.getElementById('SelectedTiles').value = '';
    submitBtn.disabled = true;

    // Enable the reset button
    resetBtn.disabled = false;
});

function updateSelectedTilesInput() {
    console.log('updateSelectedTilesInput() called');
    const selectedTiles = document.querySelectorAll('.tile.selected');
    const uids = Array.from(selectedTiles).map(tile => tile.dataset.uid);
    const input = document.getElementById('SelectedTiles');
    input.value = uids.join(',');
}