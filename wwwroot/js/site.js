document.addEventListener('DOMContentLoaded', () => {
    const authButton = document.getElementById("auth-button");
    if (authButton) authButton.addEventListener('click', authButtonClick);

    const saveProfileButton = document.getElementById("profile-save-button");
    if (saveProfileButton) saveProfileButton.addEventListener('click', saveProfileButtonClick);

    const signoutButton = document.getElementById("auth-signout-button");
    if (signoutButton) signoutButton.addEventListener('click',
        function () {
            // Выводим диалоговое окно с подтверждением
            const confirmed = confirm('Ви впевнені, що хочете вийти?');

            // Если пользователь подтвердил выход, отправляем DELETE-запрос
            if (confirmed) {
                signOutButtonClick();
            }
        });
       

    

});

function saveProfileButtonClick() {
    const nameInput = document.querySelector('input[name="profile-name"]');
    if (!nameInput) throw 'Element input[name="profile-name"] not found';
    const emailInput = document.querySelector('input[name="profile-email"]');
    if (!emailInput) throw 'Element input[name="profile-email"] not found';
    fetch(
        `/user/UpdateProfile?newName=${nameInput.value}&newEmail=${emailInput.value}`,
        {
            method: 'POST'
        })
        .then(r => r.json())
        .then(j => {
            console.log(j);
        });
}

function signOutButtonClick() {
   
    fetch('/api/auth', {
            method: 'DELETE',
            headers: {
                'Content-Type': 'application/json'
            }
        })
            .then(response => {
                if (response.ok) {
                    // Успешный выход - перезагрузка страницы или выполнение других действий
                    showSignoutMessage();
                   
                   
                } else {
                    return response.json();
                }
            })
            .then(data => {
                // Обработка данных, если необходимо
                console.log(data);
                showSignoutMessage();
            })
            .catch(error => {
                console.error('Ошибка:', error);
            });
    
    
}



function authButtonClick() {
    const loginInput = document.getElementById("auth-login");
    if (!loginInput) throw "Element #auth-login not found";
    const login = loginInput.value.trim();
    if (login.length == 0) {
        showAuthMessage("Логін не може бути порожнім");
        return;
    }
    const passwordInput = document.getElementById("auth-password");
    if (!passwordInput) throw "Element #auth-password not found";
    const password = passwordInput.value.trim();
    if (password.length == 0) {
        showAuthMessage("Пароль не може бути порожнім");
        return;
    }
    fetch(`/api/auth?login=${login}&password=${password}`)
        .then(r => {
            if (r.status == 200) {  // OK
                window.location.reload();
            }
            else {  // 401
                showAuthMessage( 5000);
            }
        });
}





function showAuthMessage(message, delay) {
    const authMessage = document.getElementById("auth-message");
    if (!authMessage) throw "Element #auth-message not found";
    authMessage.innerText = message;
    authMessage.classList.remove("visually-hidden");
}

function showSignoutMessage() {
    const signoutMessage = document.getElementById("signout-message");
    if (!signoutMessage) throw "Element #signout-message not found";
    signoutMessage.innerText = "Вихід здійснено успішно";

    signoutMessage.classList.add("alert");
    signoutMessage.classList.add("alert-success");
   

    // Задержка перед скрытием сообщения
    setTimeout(function () {
        window.location.reload();
    }, 3000);

   
}

