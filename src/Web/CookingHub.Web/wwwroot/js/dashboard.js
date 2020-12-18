﻿let ctxL = document.getElementById("lineChart").getContext('2d');
let ctxt = document.getElementById("RecipesnArticlesChar").getContext('2d');
let ctxq = document.getElementById("additional").getContext('2d');

$(document).ready(function () {
    $.ajax({
        url: '/api/statistics',
        type: 'GET',
        dataType: "json",
        success: function (data) {
            myLineChart.data.datasets[1].data = data.registeredUsers;
            myDoughnutChart.data.datasets[0].data = [data.recipesCount, data.articlesCount];
            mypolarareaChart.data.datasets[0].data = [data.reviewsCount, data.commentsCount];
            myLineChart.update();
            myDoughnutChart.update();
            mypolarareaChart.update();
        }
    })
});

let myLineChart = new Chart(ctxL, {
    type: 'line',
    data: {
        labels: ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"],
        datasets: [{
            label: "Average Daily Users",
            data: [0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0],
            borderColor: [
                'rgba(200, 99, 132, .7)',
            ],
            borderWidth: 3
        },
        {
            label: "Registered Users",
            data: [0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0],
            borderColor: [
                'rgba(0, 10, 130, .7)',
            ],
            borderWidth: 3
        }
        ]
    },
    options: {
        responsive: true
    }
});

let myDoughnutChart = new Chart(ctxt, {
    type: 'pie',
    data: {
        labels: ["Recipes", "Articles"],
        datasets: [{

            data: [0, 0],
            backgroundColor: ["#3e95cd", "#c45850"],
        }]
    },
    options: {
        responsive: false
    }
});

let mypolarareaChart = new Chart(ctxq, {
    type: 'bar',
    data: {
        labels: ["Reviews", "Article Comments"],
        datasets: [{
            label: "Reviews",
            data: [0, 0],
            backgroundColor: ["#52BE80", "#F7DC6F"],
        },
        {
            label: "Article Comments",
            data: [0, 0],
            backgroundColor: ["#F7DC6F", "#F7DC6F"],
        }]
    },
    options: {
        responsive: true
    }
});