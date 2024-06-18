//Chamando função do CSS para abrir o Formulario administrativo
function abrirFormulario() {
    document.getElementById("overlay").style.display = "block";
}
//Fechando Formulário
function fecharFormulario() {
    document.getElementById("overlay").style.display = "none";
}
//função que recebe e passa os parametros e assim sendo possivel a alteração dos formulários.
function alterarConteudo(event) {
    event.preventDefault();
    //Pegando novos dados inseridos do painel adm e passando para uma variável 
    let tema = document.getElementById("Tema").value;
    let descricao = document.getElementById("Descricao").value;
    let duracaoHr = document.getElementById("DuracaoHr").value;
    let dataFimValue = document.getElementById("DataFim").value; // yyyy-MM-dd
    let pontos = document.getElementById("Pontos").value;
    let linkCurso = document.getElementById("LinkCurso").value;
    // Parse the date value to ensure it's in the correct format
    let dataFim = new Date(dataFimValue);

    if (isNaN(dataFim.getTime())) {
        alert("Data inválida. Use o formato yyyy-MM-dd.");
        return;
    }

    //Criando objeto para obter os dados e passar atráves do AJAX
    let data = {
        Tema: tema,
        Descricao: descricao,
        DuracaoHr: duracaoHr,
        DataFim: dataFim.toISOString().split('T')[0], // Formato ISO 8601 (yyyy-MM-dd)
        Pontos: parseInt(pontos),
        LinkCurso: linkCurso
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
                alert("Alterado com sucesso")
            } else {
                console.error('Erro na validação dos dados.');
            }
        },
        error: function (error) {
            console.error('Erro ao enviar dados:', error);
        }
    });
}

