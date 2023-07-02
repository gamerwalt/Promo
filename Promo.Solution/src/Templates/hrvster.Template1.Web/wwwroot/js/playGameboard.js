const MAX_SELECTED_TILES = 3;
const submitBtn = document.querySelector('button[type="submit"]');
const resetBtn = document.getElementById('reset');
let selectedTilesCount = 0;

document.querySelectorAll('.tile').forEach(tile => {
    tile.addEventListener('click', () => {
        if (selectedTilesCount < MAX_SELECTED_TILES || tile.classList.contains('selected')) {
            tile.classList.toggle('selected');
            updateSelectedTilesInput();
            selectedTilesCount = document.querySelectorAll('.tile.selected').length;
        }

        if (selectedTilesCount === MAX_SELECTED_TILES) {
            submitBtn.disabled = false;
        } else {
            submitBtn.disabled = true;
        }

        resetBtn.disabled = selectedTilesCount === 0;
    });
});

resetBtn.addEventListener('click', () => {
    document.querySelectorAll('.tile').forEach(tile => {
        tile.classList.remove('selected');
    });
    document.getElementById('SelectedTiles').value = '';
    selectedTilesCount = 0;
    submitBtn.disabled = true;
    resetBtn.disabled = true;
});

function updateSelectedTilesInput() {
    const selectedTiles = document.querySelectorAll('.tile.selected');
    const uids = Array.from(selectedTiles).map(tile => tile.dataset.uid);
    const input = document.getElementById('SelectedTiles');
    input.value = uids.join(',');
}