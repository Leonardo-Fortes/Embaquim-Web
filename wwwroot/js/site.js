function abrirFormulario() {
    document.getElementById("overlay").style.display = "block";
}

function fecharFormulario() {
    document.getElementById("overlay").style.display = "none";
}

function alterarConteudo() {
    event.preventDefault();
    let tema = document.getElementById("Tema").value;
    let descricao = document.getElementById("Descricao").value
    let duracaoHr = document.getElementById("DuracaoHr").value;
    let duracaoMin = document.getElementById("DuracaoMin").value;
    let pontos = document.getElementById("Pontos").value;


    document.getElementById("AntTema").innerHTML = tema;
    document.getElementById("AntDescricao").innerHTML = descricao;
    document.getElementById("AntDuracaoHr").innerHTML = duracaoHr;
    document.getElementById("AntDuracaoMin").innerHTML = duracaoMin;
    document.getElementById("AntPontos").innerHTML = pontos;

}
