function abrirFormulario() {
    document.getElementById("overlay").style.display = "block";
}

function fecharFormulario() {
    document.getElementById("overlay").style.display = "none";
}

function alterarConteudo(event) {
    event.preventDefault();

    let tema = document.getElementById("Tema").value;
    let descricao = document.getElementById("Descricao").value;
    let duracaoHr = document.getElementById("DuracaoHr").value;
    let dataFimValue = document.getElementById("DataFim").value; // yyyy-MM-dd
    let pontos = document.getElementById("Pontos").value;

    // Parse the date value to ensure it's in the correct format
    let dataFim = new Date(dataFimValue);

    if (isNaN(dataFim.getTime())) {
        alert("Data inválida. Use o formato yyyy-MM-dd.");
        return;
    }

    document.getElementById("AntTema").innerHTML = tema;
    document.getElementById("AntDescricao").innerHTML = descricao;
    document.getElementById("AntDuracaoHr").innerHTML = duracaoHr;
    document.getElementById("AntPontos").innerHTML = pontos;

    let data = {
        Tema: tema,
        Descricao: descricao,
        DuracaoHr: duracaoHr,
        DataFim: dataFim.toISOString().split('T')[0], // Formato ISO 8601 (yyyy-MM-dd)
        Pontos: parseInt(pontos)
    };

    // Enviar dados para o controlador usando AJAX
    $.ajax({
        url: '/Home/SalvarCurso',
        type: 'POST',
        data: JSON.stringify(data),
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        success: function (response) {
            if (response.success) {
                console.log('Dados enviados com sucesso!');
            } else {
                console.error('Erro na validação dos dados.');
            }
        },
        error: function (error) {
            console.error('Erro ao enviar dados:', error);
        }
    });
}

