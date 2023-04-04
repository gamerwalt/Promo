const MAX_SELECTED_TILES = 3;

document.querySelectorAll('.tile').forEach(tile => {
    tile.addEventListener('click', () => {
        const selectedTiles = document.querySelectorAll('.tile.selected');
        if (selectedTiles.length < MAX_SELECTED_TILES || tile.classList.contains('selected')) {
            tile.classList.toggle('selected');
            updateSelectedTilesInput();
        }
    });
});

document.getElementById('reset').addEventListener('click', () => {
    document.querySelectorAll('.tile').forEach(tile => {
        tile.classList.remove('selected');
    });
    document.getElementById('SelectedTiles').value = '';
});

function updateSelectedTilesInput() {
    const selectedTiles = document.querySelectorAll('.tile.selected');
    const uids = Array.from(selectedTiles).map(tile => tile.dataset.uid);
    const input = document.getElementById('SelectedTiles');
    input.value = uids.join(',');
}
