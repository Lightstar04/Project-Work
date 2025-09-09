window.selectedRows = new Set();


function setTheme(name) {
    document.body.classList.toggle('dark-theme', name === 'dark')
    localStorage.setItem('theme', name);
}
document.addEventListener('DOMContentLoaded', () => {
    const theme = localStorage.getItem('theme') || 'light';
    setTheme(theme);
});



document.addEventListener('DOMContentLoaded', () => {
    const f = document.getElementById('itemsFilter');
    if (!f) return;
    f.addEventListener('input', () => {
        const q = f.value.toLowerCase();
        const rows = document.querySelectorAll('#itemsTable tbody tr');
        rows.forEach(r => {
            const text = r.innerText.toLowerCase();
            r.style.display = text.includes(q) ? '' : 'none';
        });
    });
});
