document.addEventListener("DOMContentLoaded", function () {
   // var UserId = "6a60e1ff-44bc-434b-97e4-29909c05dc83";

    fetch('/Home/GetChartData')
        .then(response => response.json())
        .then(data => {
            new Chart(document.getElementById("MyBarChartCanvas"), {
                type: "bar",
                data: data,
                options: {
                    scales: {
                        yAxes: [{
                            gridLines: {
                                display: false
                            },
                            stacked: false
                        }],
                        xAxes: [{
                            stacked: false,
                            gridLines: {
                                color: "transparent"
                            }
                        }]
                    }
                }
            });
        })
        .catch(error => console.error('Error fetching data:', error));
});

document.addEventListener("DOMContentLoaded", function () {
    fetch('/Home/GetChartData')
        .then(response => response.json())
        .then(data => {
            new Chart(document.getElementById("MyPieChartCanvas"), {
                type: "pie",
                data: {
                    labels: data.labels, 
                    datasets: [{
                        data: data.datasets[0].data, 
                        backgroundColor: data.datasets[0].backgroundColor,  
                    }]
                },
                options: {
                }
            });
        })
        .catch(error => console.error('Error fetching data:', error));
});

