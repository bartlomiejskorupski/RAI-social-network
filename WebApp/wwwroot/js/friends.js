
createFriendList();

const addFriendButton = document.querySelector('#addFriendButton');
addFriendButton.addEventListener('click', async function () {
    const loginInput = document.querySelector('#loginInput');
    const login = loginInput?.value?.trim();
    if (!login) {
        return;
    }
    loginInput.value = '';
    const formData = new FormData();
    formData.append('login', login);
    const res = await fetch('/Friends/Add', {
        method: 'POST',
        body: formData
    });
    const success = await res.json();
    console.log('Success:', success);

    if (success) {
        createFriendList();
    }
})

async function createFriendList() {
    const res = await fetch('/Friends/List', { method: 'GET' });
    const friendList = await res.json();
    console.log('Friendlist:', friendList);

    const tbody = document.querySelector('#userTableBody');
    tbody.innerHTML = '';

    if (friendList.length <= 0) {
        const p = document.createElement('p');
        p.innerHTML = 'Empty... 😥';
        tbody.appendChild(p);
    }

    for (const friend of friendList) {
        var friendRow = createFriendTableRow(friend);
        tbody.appendChild(friendRow);
    }
}

function createFriendTableRow(login) {
    const row = document.createElement('tr');
    const loginCell = document.createElement('td');
    loginCell.innerHTML = login;
    const actionsCell = document.createElement('td');
    const deleteBtn = document.createElement('button');
    deleteBtn.innerHTML = 'Delete';
    deleteBtn.classList.add('btn');
    deleteBtn.classList.add('btn-danger');
    deleteBtn.addEventListener('click', deleteFriend(login));
    actionsCell.appendChild(deleteBtn);
    row.appendChild(loginCell);
    row.appendChild(actionsCell);
    return row;
}

function deleteFriend(login) {
    return async (e) => {
        const res = await fetch(`/Friends/Del?login=${login}`, {
            method: 'DELETE'
        });
        const success = await res.json();

        if (!success) {
            console.log('Failed to delete');
            return;
        }

        const row = e.target?.parentElement?.parentElement;
        row?.remove();
    };
}