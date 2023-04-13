

$(document).ready(function () {

    // $("tr[role='row']").each(function () {
    $(".js-display-playerlink").click(function (e) {
        $this = $(this);
        const playerId = $this.data("target");
        const $playerLink = $(`tr[data-role='player-linker'][data-playerid='${playerId}']`);
        $playerLink.toggleClass("d-none");

        // document.querySelector(`tr[data-role='player-linker'][data-playerid='${playerId}']`).toggle("d-none");
    });

});