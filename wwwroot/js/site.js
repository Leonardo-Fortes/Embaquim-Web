

function abrirFormularioLogin() {
    document.getElementById("overlay-login").style.display = "block";
}

function fecharFormularioLogin() {
    document.getElementById("overlay-login").style.display = "none";
}

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

// Controle Usuário

function EnviarCadastro() {
    // Obter os valores do formulário
    const nome = document.getElementById('NomeCad').value;
    const sobrenome = document.getElementById('SobrenomeCad').value;
    const cpf = document.getElementById('CpfCad').value;
    const dataNascimento = document.getElementById('DataNasciCad').value;
    const funcao = document.getElementById('FuncaoCad').value;
    const email = document.getElementById('EmailCad').value;
    const usuario = document.getElementById('UsuarioCad').value;
    const senha = document.getElementById('SenhaCad').value;
    const confSenha = document.getElementById('ConfSenhaUsuario').value;


    // Verificar se todos os campos foram preenchidos
    if (!nome || !sobrenome || !cpf || !dataNascimento || !funcao || !email || !usuario || !senha || !confSenha) {
        alert("Por favor, preencha todos os campos.");
        return;
    }
    if (confSenha != senha) {
        alert("Confirmar senha deve estar igual a senha!");
        return;
    }

    // Armazenar os dados no localStorage
    const solicitacoes = JSON.parse(localStorage.getItem('solicitacoes')) || [];
    solicitacoes.push({ nome, sobrenome, cpf, dataNascimento, funcao, email });
    localStorage.setItem('solicitacoes', JSON.stringify(solicitacoes));

    // Limpar o formulário original
    document.getElementById('userForm').reset();

    // Redirecionar para a página de solicitações

}


document.addEventListener('DOMContentLoaded', function () {
    const solicitacoesContainer = document.getElementById('solicitationsContainer');
    const solicitacoes = JSON.parse(localStorage.getItem('solicitacoes')) || [];

    solicitacoes.forEach((solicitacao, index) => {
        const newForm = document.createElement('form');
        newForm.className = 'input-cad';
        newForm.innerHTML = `
                    <div class="d-flex justify-content-between">
                        <div class="d-flex flex-column">
                            <label for="NomeCad">Nome </label>
                            <input class="" type="text" name="NomeCad" value="${solicitacao.nome}" disabled><br>
                        </div>
                        <div class="d-flex flex-column">
                            <label for="SobrenomeCad">Sobrenome </label>
                            <input type="text" name="SobrenomeCad" value="${solicitacao.sobrenome}" disabled><br>
                        </div>
                        <div class="d-flex flex-column">
                            <label for="CpfCad">CPF</label>
                            <input type="text" name="CpfCad" value="${solicitacao.cpf}" disabled><br>
                        </div>
                    </div>
                    <div class="d-flex justify-content-between">
                        <div class="d-flex flex-column">
                            <label for="DataNasciCad">Data de nascimento</label>
                            <input type="date" name="DataNasciCad" value="${solicitacao.dataNascimento}" disabled><br>
                        </div>
                        <div class="d-flex flex-column">
                            <label for="FuncaoCad">Função </label>
                            <input type="text" name="FuncaoCad" value="${solicitacao.funcao}" disabled><br>
                        </div>
                        <div class="d-flex flex-column">
                            <label for="EmailCad">Email </label>
                            <input type="email" name="EmailCad" value="${solicitacao.email}" disabled><br>
                        </div>
                    </div>
                    <div class="d-flex justify-content-end">
                        <button type="button" class="btn btn-danger mx-2" onclick="recusarSolicitacao(${index})">Recusar</button>
                        <button type="button" class="btn btn-primary d-flex" onclick="aceitarSolicitacao()">Aceitar</button>
                    </div>
                    <hr>
                `;
        solicitacoesContainer.appendChild(newForm);
    });
});

function recusarSolicitacao(index) {
    const solicitacoes = JSON.parse(localStorage.getItem('solicitacoes')) || [];
    solicitacoes.splice(index, 1);
    localStorage.setItem('solicitacoes', JSON.stringify(solicitacoes));
    window.location.reload();  // Recarrega a página para atualizar a lista
}

function aceitarSolicitacao() {
    // Obter as solicitações do localStorage
    const solicitacoes = JSON.parse(localStorage.getItem('solicitacoes')) || [];

    // Verificar se há solicitações para enviar
    if (solicitacoes.length === 0) {
        alert("Nenhuma solicitação encontrada.");
        return;
    }

    // Adicionar a variável extra (por exemplo, 'status') ao objeto a ser enviado
    const dataToSend = {
        Solicitacoes: solicitacoes,
        Usuario: usuario,
        Senha: senha
    };

    // Enviar dados para o controlador usando AJAX
    $.ajax({
        url: '/Home/PreCadastroUsuario',
        type: 'POST',
        data: JSON.stringify(dataToSend),
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        success: function (response) {
            if (response.success) {
                console.log('Dados enviados com sucesso!');
                alert("Alterado com sucesso");
                // Limpar o localStorage após enviar os dados com sucesso
                localStorage.removeItem('solicitacoes');
            } else {
                console.error('Erro na validação dos dados.');
            }
        },
        error: function (error) {
            console.error('Erro ao enviar dados:', error);
        }
    });
}

//Reconhecer



let idFunc = null;
let nameFunc = null;
function buscarUsuarios() {
    //Cria o prefixo vinculado ao input onde o usuário vai fazer a busca
    let prefixo = document.getElementById('buscaUsuario').value;

    let lista = document.getElementById('listaUsuarios');
    lista.innerHTML = '';

    // quando prefixo for vazio, limpa a lista
    if (prefixo === '') {
        return; // Limpa a lista e não faz a chamada AJAX
    }

    $.ajax({
        url: '/Reconhecer/BuscarUsuarios',
        type: 'GET',
        data: { prefixo: prefixo },
        success: function (response) {
            console.log(response); // Adicione este log para verificar o JSON retornado
            if (response.length === 0) {
                lista.innerHTML = '<li>Nenhum usuário encontrado</li>';
            } else {
                response.forEach(function (usuario) {
                    let li = document.createElement('li');
                    li.className = 'list-group-item list-add-rec fw-bolder';
                    li.textContent = usuario.nomeFunc;
                    li.onclick = function () {
                        document.getElementById('buscaUsuario').value = usuario.nomeFunc;
                        lista.innerHTML = ''; // Limpa a lista após selecionar um usuário
                        idFunc = usuario.id; 
                        nameFunc = usuario.nomeFunc;

                        console.log('ID Func:', idFunc);
                        console.log('Nome Func:', nameFunc);
                    };
                    lista.appendChild(li);
                });
            }
        },
        error: function (error) {
            console.error('Erro ao buscar usuários:', error);
        }
    });

}


//Reconhecer Valor das medalhas
let selectValue = null;
document.addEventListener('DOMContentLoaded', function () {
    const icons = document.querySelectorAll('.selectable-icon');
    // Variável para armazenar o valor do ícone selecionado
    const messageBox = document.getElementById('message-box');

    icons.forEach(icon => {
        icon.addEventListener('click', function () {
            // Remove a classe 'selected' de todos os ícones
            icons.forEach(i => i.classList.remove('selected'));

            // Adiciona a classe 'selected' ao ícone clicado
            this.classList.add('selected');

            // Obtém o valor do ícone clicado
            selectedValue = this.getAttribute('data-value');
            console.log('Ícone selecionado com valor:', selectedValue); // Exibe o valor no console
        });

        icon.addEventListener('mouseover', function (event) {
            const message = this.getAttribute('data-message');
            messageBox.innerHTML = message;
            messageBox.style.display = 'block';
            messageBox.style.left = event.pageX + 'px';
            messageBox.style.top = event.pageY + 'px';
        });

        icon.addEventListener('mouseout', function () {
            messageBox.style.display = 'none';
        });
    });
});

document.addEventListener('DOMContentLoaded', function () {
    const valorDefinidoInput = document.getElementById('pontosDis');
    const valorUsuarioInput = document.getElementById('pontosEnv');

    valorUsuarioInput.addEventListener('input', function () {
        const valorDefinido = parseFloat(valorDefinidoInput.value);
        let valorUsuario = parseFloat(valorUsuarioInput.value);

        if (valorUsuario > valorDefinido || valorUsuario < 1) {
            valorUsuarioInput.value = valorDefinido;
            alert('Confira sua quantidade de pontos !');
        }

    });
});

function enviarReconhecer() {
    const msgRec = document.getElementById("msgRec").value;
    const pontosEnv = document.getElementById("pontosEnv").value;

    console.log(msgRec);
    console.log('Ícone selecionado com valor:', selectedValue)
    console.log('Id selecionado', idFunc);

    if (!msgRec || !selectedValue || !idFunc) {
        alert("Por favor, preencha todos os campos.");
        return;
    }

    //else if (pontosEnv === 0) {
    //    alert("Por favor, insira a quantidade de pontos.");
    //    return;
    //}
    else {
        let data = {
            IdFuncRec: idFunc,
            Medalha: selectedValue,
            Nome: nameFunc,
            Pontos: pontosEnv,
            Msg: msgRec
        };

        $.ajax({
            url: '/Reconhecer/EnviarReconhecer',
            type: 'POST',
            data: JSON.stringify(data),
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            success: function (response) {
                if (response.success) {
                    console.log('Dados enviados com sucesso!');
                    alert("Alterado com sucesso");
                    window.location.reload();
                } else {
                    console.error('Erro na validação dos dados.');
                    alert(response.message); // Exibe a mensagem de erro retornada pelo servidor
                }
            },
            error: function (error) {
                console.error('Erro ao enviar dados:', error);
            }
        });
    }
}
