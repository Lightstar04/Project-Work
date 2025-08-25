window.selectedRows = new Set();

function toggleSelect(rowEl, id) {
    const checkbox = rowEl.querySelector('input[type="checkbox"]');
    if (rowEl.classList.contains('selected')) {
        rowEl.classList.remove('selected');
        if (checkbox) checkbox.checked = false;
        window.selectedRows.delete(id);
    } else {
        rowEl.classList.add('selected');
        if (checkbox) checkbox.checked = true;
        window.selectedRows.add(id);
    }
    updateContextToolbar();
}

function updateContextToolbar() {
    const toolbar = document.getElementById('contextToolbar');
    const btns = document.getElementById('contextToolbarButtons');
    const count = document.getElementById('contextCount');
    btns.innerHTML = '';
    count.innerText = window.selectedRows.size;
    if (window.selectedRows.size === 0) {
        toolbar.classList.add('d-none');
    } else {
        toolbar.classList.remove('d-none');

        // Delete action
        const deleteBtn = document.createElement('button');
        deleteBtn.className = 'btn btn-danger';
        deleteBtn.innerText = 'Delete';
        deleteBtn.onclick = () => {
            if (!confirm('Delete selected items?')) return;
            alert('Implement bulk delete API for: ' + Array.from(window.selectedRows).join(', '));
        };
        btns.appendChild(deleteBtn);

        // Edit action (open first selected)
        const editBtn = document.createElement('button');
        editBtn.className = 'btn btn-primary';
        editBtn.innerText = 'Edit';
        editBtn.onclick = () => {
            const id = Array.from(window.selectedRows)[0];
            // open item edit page or drawer
            alert('Open editor for: ' + id);
        };
        btns.appendChild(editBtn);

        // More actions dropdown
        const more = document.createElement('div');
        more.className = 'btn-group';
        more.innerHTML = `<button class="btn btn-secondary dropdown-toggle" data-bs-toggle="dropdown">More</button>
            <ul class="dropdown-menu"><li><a class="dropdown-item" href="#">Export</a></li></ul>`;
        btns.appendChild(more);
    }
}

// Theme management
function setTheme(name) {
    document.body.classList.toggle('dark-theme', name === 'dark')
    localStorage.setItem('theme', name);
}
document.addEventListener('DOMContentLoaded', () => {
    const theme = localStorage.getItem('theme') || 'light';
    setTheme(theme);
});

// Simple client-side filter for items table
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
