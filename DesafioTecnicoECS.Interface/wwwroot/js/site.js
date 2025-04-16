/* Carregameto do sistema
-------------------------------------------------- */
$(document).ready(function () {
    $('#contentCandidato').hide();
    $('#criarCandidato').hide();
    $('#alert').hide();
});

/* Votacao
-------------------------------------------------- */

$('#box1').keyup(function () {
    $('#box2').focus();
});

$('.votacao').keyup(function () {
    const digito1 = $('#box1').val();
    const digito2 = $('#box2').val();
    if (digito1 !== "") {
        $('#box1').prop('readonly', true);

    }
    if (digito2 !== "") {
        $('#box2').prop('readonly', true);
    }
    if (digito1 !== "" && digito2 !== "") {
        const legenda = digito1 + digito2;
        $.ajax({
            url: '/BuscarCandidato/' + parseInt(legenda),
            method: 'POST',
            success: function (data) {
                console.log(data);
                $('#nomeCandidato').html(data.nomeCandidato + '<p class="text-muted">Vice: ' + data.viceCandidato + '</p>');
                $('#legendaCandidato').html(data.legenda);
                $('#contentCandidato').fadeIn();
            }
        });
    }
});
$('#corrige').click(function () {
    $('#box1').val("");
    $('#box2').val("");
    $('#box1').prop('readonly', false);
    $('#box2').prop('readonly', false);
    $('#contentCandidato').fadeOut();
});
$('#branco').click(function () {
    const obj = {};
    obj.IdVoto = 0;
    obj.IdCandidato = "";
    obj.NomeCandidato = "";

    $.ajax({
        url: '/VotoFinalizado',
        method: 'POST',
        data: JSON.stringify(obj),
        dataType: 'json',
        contentType: 'application/json',
        success: function (data) {
            if (data) {
                $('#alert').addClass('alert-success');
                $('#alert').append('Voto computado com sucesso!');
                $('#alert').fadeIn();
            } else {
                $('#alert').addClass('alert-danger');
                $('#alert').append('Tivemos problemas tente novamente!');
                $('#alert').fadeIn();
            }
        }
    });
});
$('#confirma').click(function () {
    const digito1 = $('#box1').val();
    const digito2 = $('#box2').val();
    const obj = {};
    obj.IdVoto = 0;
    obj.IdCandidato = parseInt(digito1 + digito2).toString();
    obj.NomeCandidato = "";

    if (digito1 === "" || digito2 === "") {
        alert('E necessário inserir a legenda do candidato para confirma o voto!');
        return false;
    }

    $.ajax({
        url: '/VotoFinalizado',
        method: 'POST',
        data: JSON.stringify(obj),
        dataType: 'json',
        contentType: 'application/json',
        success: function (data) {
            if (data) {
                $('#alert').addClass('alert-success');
                $('#alert').append('Voto computado com sucesso!');
                $('#alert').fadeIn();
            } else {
                $('#alert').addClass('alert-danger');
                $('#alert').append('Tivemos problemas tente novamente!');
                $('#alert').fadeIn();
            }
            $('#corrige').click();
        }
    });
});

/* Tela de Candidato
-------------------------------------------------- */
$('#novoCandidato').click(function () {
    $('#criarCandidato').fadeIn();
});
$('#addCandidato').click(function (e) {
    e.preventDefault();
    const form = document.forms.criarCandidato;

    const { nome, IdLegenda, vice } = form;
    if (nome.value === "") {
        alert('E necessário adicionar o nome do candidato!');
        return false;
    }
    if (vice.value === "") {
        alert('E necessário adicionar o nome do vice candidato!');
        return false;
    }
    if (IdLegenda.value === "") {
        alert('E necessário adicionar o partido do candidato!');
        return false;
    }

    const obj = {};
    obj.IdCandidato = "";
    obj.NomeCandidato = nome.value;
    obj.ViceCandidato = vice.value;
    obj.Legenda = IdLegenda.value; //numero do partido

    $.ajax({
        url: '/CadastrarCandidato',
        method: 'POST',
        data: JSON.stringify(obj),
        dataType: 'json',
        contentType: 'application/json',
        success: function (data) {
            if (data) {
                $('#alertCandidato').addClass('alert-success');
                $('#alertCandidato').html('Candidato cadastrado com sucesso!');
                $('#alertCandidato').fadeIn();
            } else {
                $('#alertCandidato').addClass('alert-danger');
                $('#alertCandidato').html('Tivemos problemas tente novamente!');
                $('#alertCandidato').fadeIn();
            }
            $('#IdLegenda').val("");
            $('#nomeCandidato').val("");
            $('#viceCandidato').val("");
            setTimeout(function () {
                $('#criarCandidato').fadeOut();
                $('#alertCandidato').fadeOut();
            }, 3000);
        }
    });

});
function deletarCandidato(id) {
    $.ajax({
        url: '/DeletarCandidato/' + id,
        method: 'POST',
        success: function (data) {
            if (data) {
                $('#alertCandidato').addClass('alert-success');
                $('#alertCandidato').html('Candidato deletado com sucesso!');
                $('#alertCandidato').fadeIn();
            } else {
                $('#alertCandidato').addClass('alert-danger');
                $('#alertCandidato').html('Tivemos problemas tente novamente!');
                $('#alertCandidato').fadeIn();
            }
            setTimeout(function () {
                $('#alertCandidato').fadeOut();
            }, 2000);
        }
    });
}