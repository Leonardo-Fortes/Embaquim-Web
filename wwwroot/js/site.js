
//Curso Home
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

//function EnviarCadastro() {
//    // Obter os valores do formulário
//    const nome = document.getElementById('NomeCad').value;
//    const sobrenome = document.getElementById('SobrenomeCad').value;
//    const cpf = document.getElementById('CpfCad').value;
//    const dataNascimento = document.getElementById('DataNasciCad').value;
//    const funcao = document.getElementById('FuncaoCad').value;
//    const email = document.getElementById('EmailCad').value;
//    const usuario = document.getElementById('UsuarioCad').value;
//    const senha = document.getElementById('SenhaCad').value;
//    const confSenha = document.getElementById('ConfSenhaUsuario').value;


//    // Verificar se todos os campos foram preenchidos
//    if (!nome || !sobrenome || !cpf || !dataNascimento || !funcao || !email || !usuario || !senha || !confSenha) {
//        alert("Por favor, preencha todos os campos.");
//        return;
//    }
//    if (confSenha != senha) {
//        alert("Confirmar senha deve estar igual a senha!");
//        return;
//    }

//    // Armazenar os dados no localStorage
//    const solicitacoes = JSON.parse(localStorage.getItem('solicitacoes')) || [];
//    solicitacoes.push({ nome, sobrenome, cpf, dataNascimento, funcao, email });
//    localStorage.setItem('solicitacoes', JSON.stringify(solicitacoes));

//    // Limpar o formulário original
//    document.getElementById('userForm').reset();

//    // Redirecionar para a página de solicitações
//}


//document.addEventListener('DOMContentLoaded', function () {
//    const solicitacoesContainer = document.getElementById('solicitationsContainer');
//    const solicitacoes = JSON.parse(localStorage.getItem('solicitacoes')) || [];

//    solicitacoes.forEach((solicitacao, index) => {
//        const newForm = document.createElement('form');
//        newForm.className = 'input-cad';
//        newForm.innerHTML = `
//                    <div class="d-flex justify-content-between">
//                        <div class="d-flex flex-column">
//                            <label for="NomeCad">Nome </label>
//                            <input class="" type="text" name="NomeCad" value="${solicitacao.nome}" disabled><br>
//                        </div>
//                        <div class="d-flex flex-column">
//                            <label for="SobrenomeCad">Sobrenome </label>
//                            <input type="text" name="SobrenomeCad" value="${solicitacao.sobrenome}" disabled><br>
//                        </div>
//                        <div class="d-flex flex-column">
//                            <label for="CpfCad">CPF</label>
//                            <input type="text" name="CpfCad" value="${solicitacao.cpf}" disabled><br>
//                        </div>
//                    </div>
//                    <div class="d-flex justify-content-between">
//                        <div class="d-flex flex-column">
//                            <label for="DataNasciCad">Data de nascimento</label>
//                            <input type="date" name="DataNasciCad" value="${solicitacao.dataNascimento}" disabled><br>
//                        </div>
//                        <div class="d-flex flex-column">
//                            <label for="FuncaoCad">Função </label>
//                            <input type="text" name="FuncaoCad" value="${solicitacao.funcao}" disabled><br>
//                        </div>
//                        <div class="d-flex flex-column">
//                            <label for="EmailCad">Email </label>
//                            <input type="email" name="EmailCad" value="${solicitacao.email}" disabled><br>
//                        </div>
//                    </div>
//                    <div class="d-flex justify-content-end">
//                        <button type="button" class="btn btn-danger mx-2" onclick="recusarSolicitacao(${index})">Recusar</button>
//                        <button type="button" class="btn btn-primary d-flex" onclick="aceitarSolicitacao()">Aceitar</button>
//                    </div>
//                    <hr>
//                `;
//        solicitacoesContainer.appendChild(newForm);
//    });
//});

//function recusarSolicitacao(index) {
//    const solicitacoes = JSON.parse(localStorage.getItem('solicitacoes')) || [];
//    solicitacoes.splice(index, 1);
//    localStorage.setItem('solicitacoes', JSON.stringify(solicitacoes));
//    window.location.reload();  // Recarrega a página para atualizar a lista
//}

//function aceitarSolicitacao() {
//    // Obter as solicitações do localStorage
//    const solicitacoes = JSON.parse(localStorage.getItem('solicitacoes')) || [];

//    // Verificar se há solicitações para enviar
//    if (solicitacoes.length === 0) {
//        alert("Nenhuma solicitação encontrada.");
//        return;
//    }

//    // Adicionar a variável extra (por exemplo, 'status') ao objeto a ser enviado
//    const dataToSend = {
//        Solicitacoes: solicitacoes,
//        Usuario: usuario,
//        Senha: senha
//    };

//    // Enviar dados para o controlador usando AJAX
//    $.ajax({
//        url: '/Home/PreCadastroUsuario',
//        type: 'POST',
//        data: JSON.stringify(dataToSend),
//        contentType: 'application/json; charset=utf-8',
//        dataType: 'json',
//        success: function (response) {
//            if (response.success) {
//                console.log('Dados enviados com sucesso!');
//                alert("Alterado com sucesso");
//                // Limpar o localStorage após enviar os dados com sucesso
//                localStorage.removeItem('solicitacoes');
//            } else {
//                console.error('Erro na validação dos dados.');
//            }
//        },
//        error: function (error) {
//            console.error('Erro ao enviar dados:', error);
//        }
//    });
//}

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

//Barra de pesquisa

function PesquisaPerfil() {
    //Cria o prefixo vinculado ao input onde o usuário vai fazer a busca
    let prefixo = document.getElementById('pesquisaPerfil').value;

    let lista = document.getElementById('listaUsuariosPerfil');
    lista.innerHTML = '';

    // quando prefixo for vazio, limpa a lista
    if (prefixo === '') {
        return; // Limpa a lista e não faz a chamada AJAX
    }

    $.ajax({
        url: '/Home/PesquisaPerfil',
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
                        document.getElementById('pesquisaPerfil').value = usuario.nomeFunc;
                        lista.innerHTML = ''; // Limpa a lista após selecionar um usuário
                        let idFunc = usuario.id;
                        let nameFunc = usuario.nomeFunc;

                        console.log('ID Func:', idFunc);
                        console.log('Nome Func:', nameFunc);

                        // Redireciona para a view Perfil passando o ID do usuário
                        window.location.href = '/Perfil/Index?id=' + idFunc;
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

//Pontos Reconhecimento
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

//Reconhecimento
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

//Perfil Historico de Reconhecimento

$(document).ready(function () {
    $.ajax({
        url: '/Perfil/GetData',
        type: 'GET',
        success: function (data) {
            console.log(data);
            // Verifica se a resposta contém dados e se não há erro
            if (Array.isArray(data) && data.length > 0) {
                var content = '';
                $.each(data, function (index, item) {
                    content += '<div class="dataItem fundo-branco-comentario" style="margin-top: 40px; margin-bottom: 40px;">';
                    content += '<div class="d-flex"><div class="d-flex"><img src="' + item.fotoUrl + '" alt="Foto de Perfil" class="img-perfil-rec rounded-circle" style="width: 80px; height:80px"><div class="d-flex flex-column justify-content-center fw-bolder" style="margin-left: 10px;"><span>' + item.nome + '</span> <span>' + item.cargo + '</span></div></div> <div class="d-flex flex-column justify-content-center"style="margin-left: auto"><span style="margin-left: auto"><strong>Data:</strong> ' + item.data + '</span> <span><strong>Pontos:</strong>'+ item.pontos + '</span></div></div>';
                    content += '<textarea style="color:black" id="msgRec" class="input-text-placeholder " disabled placeholder=" ' + item.texto + ' "></textarea>';
                    switch (item.medalha) {
                        case 'Coluna1': content +='<i class="fa-regular  fa-face-smile icon-rec-perfil selectable-icon" data-message="Ser Amigável: Significa ser cordial, gentil e acolhedor, demonstrando interesse genuíno e empatia nas interações com os outros."></i>';
                        break;
                        case 'Coluna2': content += '<i class="fa-regular fa-lightbulb icon-rec-perfil selectable-icon" data-message="Ser Inovador: Envolve a capacidade de criar ou introduzir algo novo e original, seja em ideias, processos ou produtos, buscando constantemente melhorias e soluções criativas."></i>';
                            break;
                        case 'Coluna3': content += '<i class="fa-solid fa-star icon-rec-perfil selectable-icon" data-message="Ser Protagonista: Significa assumir responsabilidade e iniciativa em situações, sendo um líder ativo na condução de eventos ou circunstâncias importantes."></i>';
                            break;
                        default: content += '<i class="fa-solid fa-user-tie icon-rec-perfil selectable-icon" data-message="Profissionalismo: Refere-se à conduta ética, habilidades técnicas e comportamento adequado esperados em ambientes de trabalho, demonstrando comprometimento, competência e respeito."></i>'
                            break;
                    }
                    content += '</div>';
                });
                $('#dataItems').html(content);
                $('#dataContainer').show();
            } else {
                console.log('No data found or an error occurred');
            }
        },
        error: function () {
            console.log('Erro ao buscar dados.');
        }
    });
});


//Perfil Edição Imagem

$(document).ready(function () {
    var cropper;
    var image = document.getElementById('image');

    // Exibir a imagem no modal quando o arquivo é selecionado
    $("#file").on("change", function (e) {
        var files = e.target.files;
        var done = function (url) {
            image.src = url;
            $('#cropModal').modal('show');
        };
        var reader;
        var file;

        if (files && files.length > 0) {
            file = files[0];

            if (URL) {
                done(URL.createObjectURL(file));
            } else if (FileReader) {
                reader = new FileReader();
                reader.onload = function (e) {
                    done(reader.result);
                };
                reader.readAsDataURL(file);
            }
        }
    });

    // Inicializar o Cropper.js quando o modal é exibido
    $('#cropModal').on('shown.bs.modal', function () {
        cropper = new Cropper(image, {
            aspectRatio: 1, // Proporção 1:1 para foto de perfil
            viewMode: 3,   // Controla o modo de visualização
            preview: '.preview'
        });
    }).on('hidden.bs.modal', function () {
        cropper.destroy();
        cropper = null;
    });

    // Recortar e enviar a imagem quando o botão é clicado
    $("#cropButton").on("click", function () {
        var canvas;
        if (cropper) {
            canvas = cropper.getCroppedCanvas({
                width: 300,
                height: 300,
            });
            canvas.toBlob(function (blob) {
                var reader = new FileReader();
                reader.readAsDataURL(blob);
                reader.onloadend = function () {
                    var base64data = reader.result;
                    $.ajax({
                        type: "POST",
                        dataType: "json",
                        url: '/Acesso/UploadImagem',
                        data: { image: base64data },
                        success: function (data) {
                            if (data.success) {
                                $('#cropModal').modal('hide');
                                // Atualiza a imagem de perfil na página
                                $('#profileImage').attr('src', data.imageUrl);
                            } else {
                                alert(data.message);
                            }
                        },
                        error: function (error) {
                            console.log(error);
                        }
                    });
                };
            });
        }
    });
});


// aumentar a foto de perfil quando clicado

$(document).ready(function () {
    // Função para abrir o modal
    function openModal() {
        $('#profileModal').modal('show');
    }

    // Adiciona o evento click à imagem para abrir o modal
    $('#perfilImg').on('click', function () {
        openModal();
    });
});
