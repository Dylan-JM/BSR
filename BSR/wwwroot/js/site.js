function showConfirmation() {
  document.getElementById("deleteConfirmation").style.display = "block";
}

function closeConfirmation() {
  document.getElementById("deleteConfirmation").style.display = "none";
}

function deleteHome(id) {
  fetch("/Homes/Delete/" + id, {
    method: "POST",
  }).then((response) => {
    window.location.href = window.location.origin + "/Homes/Index";
  });
}

document.addEventListener("DOMContentLoaded", function () {
  var countysDropdown = document.getElementById("countyFilter");
  var citiesDropdown = document.getElementById("cityFilter");
  var selectedCounty = countysDropdown.value;
  var selectedCity = citiesDropdown.value;

  function populateCities(selectedCounty) {
    fetch(`/Homes/GetCities?county=${selectedCounty}`)
      .then((response) => {
        if (!response.ok) {
          throw new Error("Network response was not ok");
        }
        return response.json();
      })
      .then((data) => {
        citiesDropdown.innerHTML = '<option value="">Select City</option>';
        data.forEach((city) => {
          var option = document.createElement("option");
          option.textContent = city;
          option.value = city;
          citiesDropdown.appendChild(option);
        });

        if (selectedCity && data.includes(selectedCity)) {
          citiesDropdown.value = selectedCity;
        }
      })
      .catch((error) => {
        console.error("There was a problem with the fetch operation:", error);
      });
  }

  if (selectedCounty) {
    populateCities(selectedCounty);
  }

  countysDropdown.addEventListener("change", function () {
    selectedCounty = this.value;
    if (selectedCounty) {
      populateCities(selectedCounty);
    } else {
      citiesDropdown.innerHTML =
        '<option value="">Select County First</option>';
    }
  });
});
