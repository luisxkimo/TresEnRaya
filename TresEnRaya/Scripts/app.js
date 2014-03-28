$(document).ready(function() {

    var connection = $.connection.gameHub;

    var chipType;

    $.connection.hub.start();
    $.connection.hub.logging = true;

    connection.client.chipType= function (chip) {
        chipType = chip==1 ? 'O' : 'X';

        $("#chipType").html('Juegas con:' + chipType);
    };

    connection.client.showNewMovement = function (row, col, chip) {
        $("#cell" + row + "" + col + "").html(chip == 1 ? 'O' : 'X');
        $("#board").attr('disabled', false);
    };

    connection.client.gameOver = function (winner) {
        $("#chipType").html('');
        if (winner === "")
        {
            $("#winner").html('La partida ha finalizado en tablas');
        }
        else {
            $("#winner").html('El ganador es:' + winner);
        }
        
        $("#board").attr('disabled', true);
    };

    connection.client.newGame = function () {
        $("#board").attr('disabled', false);
    };

    $(".cell").click(function () {
        if ($(this).text() == "" &&  $("#board").attr('disabled')==undefined)
        {
            $(this).html(chipType);

            connection.server.nextMovement($(this).attr("row"), $(this).attr("col"), chipType);
            $("#board").attr('disabled',true);
        }
    });

    $("#connect").click(function () {
        connection.server.connectGame($("#nombre").val());
        resetGame();
    });

    function resetGame() {
        chipType = "";
        $(".cell").html('');
        $("#winner").html('');
        $("#board").attr('disabled', true);
    }
});