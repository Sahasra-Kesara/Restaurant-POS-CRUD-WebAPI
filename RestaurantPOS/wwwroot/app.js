const apiUrl = 'http://localhost:7027/api'; // Update this to your actual API URL

// Load Foods
async function loadFoods() {
    const response = await fetch(`${apiUrl}/food`);
    const foods = await response.json();
    const foodsList = document.getElementById('foods-list');
    foodsList.innerHTML = foods.map(food =>
        `<tr>
            <td>${food.id}</td>
            <td>${food.name}</td>
            <td>${food.price}</td>
            <td>${food.description}</td>
            <td>
                <button onclick="deleteFood(${food.id})">Delete</button>
                <button onclick="editFood(${food.id}, '${food.name}', ${food.price}, '${food.description}')">Edit</button>
            </td>
        </tr>`
    ).join('');
}

// Load Beverages
async function loadBeverages() {
    const response = await fetch(`${apiUrl}/beverage`);
    const beverages = await response.json();
    const beveragesList = document.getElementById('beverages-list');
    beveragesList.innerHTML = beverages.map(beverage =>
        `<tr>
            <td>${beverage.id}</td>
            <td>${beverage.name}</td>
            <td>${beverage.price}</td>
            <td>${beverage.description}</td>
            <td>
                <button onclick="deleteBeverage(${beverage.id})">Delete</button>
                <button onclick="editBeverage(${beverage.id}, '${beverage.name}', ${beverage.price}, '${beverage.description}')">Edit</button>
            </td>
        </tr>`
    ).join('');
}

// Add New Food/Beverage
document.getElementById('add-item-form').addEventListener('submit', async function (event) {
    event.preventDefault();
    const name = document.getElementById('name').value;
    const price = parseFloat(document.getElementById('price').value);
    const description = document.getElementById('description').value;
    const type = document.getElementById('item-type').value;

    const item = { name, price, description };

    try {
        let response;
        if (type === 'food') {
            response = await fetch(`${apiUrl}/food`, {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(item)
            });
        } else {
            response = await fetch(`${apiUrl}/beverage`, {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(item)
            });
        }

        if (response.ok) {
            alert('Item added successfully');
            document.getElementById('add-item-form').reset();
            loadFoods();  // Reload foods or beverages based on the type
            loadBeverages();
        } else {
            alert('Failed to add item');
        }
    } catch (error) {
        console.error('Error:', error);
        alert('Error adding item');
    }
});

// Delete Food
async function deleteFood(id) {
    const response = await fetch(`${apiUrl}/food/${id}`, { method: 'DELETE' });
    if (response.ok) {
        alert('Food deleted successfully');
        loadFoods();
    } else {
        alert('Failed to delete food');
    }
}

// Delete Beverage
async function deleteBeverage(id) {
    const response = await fetch(`${apiUrl}/beverage/${id}`, { method: 'DELETE' });
    if (response.ok) {
        alert('Beverage deleted successfully');
        loadBeverages();
    } else {
        alert('Failed to delete beverage');
    }
}

// Edit Food
function editFood(id, name, price, description) {
    document.getElementById('name').value = name;
    document.getElementById('price').value = price;
    document.getElementById('description').value = description;
    document.getElementById('item-type').value = 'food';

    // Replace Add with Update button logic here
    const submitButton = document.querySelector('button[type="submit"]');
    submitButton.textContent = 'Update Item';
    submitButton.onclick = async function (event) {
        event.preventDefault();
        const updatedItem = {
            name: document.getElementById('name').value,
            price: parseFloat(document.getElementById('price').value),
            description: document.getElementById('description').value
        };

        const response = await fetch(`${apiUrl}/food/${id}`, {
            method: 'PUT',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(updatedItem)
        });

        if (response.ok) {
            alert('Food updated successfully');
            loadFoods();
            submitButton.textContent = 'Add Item';
            submitButton.onclick = addNewFoodOrBeverage;
        } else {
            alert('Failed to update food');
        }
    };
}

// Edit Beverage
function editBeverage(id, name, price, description) {
    document.getElementById('name').value = name;
    document.getElementById('price').value = price;
    document.getElementById('description').value = description;
    document.getElementById('item-type').value = 'beverage';

    // Replace Add with Update button logic here
    const submitButton = document.querySelector('button[type="submit"]');
    submitButton.textContent = 'Update Item';
    submitButton.onclick = async function (event) {
        event.preventDefault();
        const updatedItem = {
            name: document.getElementById('name').value,
            price: parseFloat(document.getElementById('price').value),
            description: document.getElementById('description').value
        };

        const response = await fetch(`${apiUrl}/beverage/${id}`, {
            method: 'PUT',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(updatedItem)
        });

        if (response.ok) {
            alert('Beverage updated successfully');
            loadBeverages();
            submitButton.textContent = 'Add Item';
            submitButton.onclick = addNewFoodOrBeverage;
        } else {
            alert('Failed to update beverage');
        }
    };
}

// Load foods and beverages on page load
window.onload = () => {
    loadFoods();
    loadBeverages();
};
