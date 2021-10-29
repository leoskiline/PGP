let indexLivro = {

    init: () => {
        indexLivro.pesquisar();
    },

    pesquisar: () => {

        let dados = {
            termo: document.getElementById("pesquisaLivro").value,
            tipo: document.getElementById("tipoPesquisa").value.toLowerCase()
        };
        HTTPClient.post("/Livro/Pesquisar", dados)
            .then(result => {
                return result.json();
            })
            .then(dados => {
                var table = "";
                var autores = "";
                dados.livros.forEach(item => {
                    table += `<tr>
                        <td>${item.id}</td>
                        <td>${item.nome}</td>

                        <td>${item.qtd}</td>
                        <td>${item.editora.nome}</td>
                        <td><select class="form-control">${indexLivro.carregarAutores(item.autor)}</select></td>
                        <td onclick='indexLivro.apagar(${item.id})'>X</td>
                        <td onclick='indexLivro.alterar(${item.id})'>▓</td>
                              </tr>`;
                });
                document.getElementById("corpo-tabela").innerHTML = table;

            })
            .catch(() => {
                console.log("Falha ao Pesquisar");
            });

    },

    apagar: (id) => {
        HTTPClient.delete("/Livro/Deletar?id=" + id)
            .then(function (response) {
                return response.json();
            })
            .then(dados => {
                indexLivro.pesquisar();
                alert(dados.msg);
            }).catch(function (error) {
                console.error(error);
            });
    },

    carregarAutores: (autores) => {
        var option = "";
        autores.forEach(item => {
            option += `<option value="${item.id}">${item.nome}</option>`;
        });
        return option;
    },

    alterar: (id) => {
        HTTPClient.put("/Livro/Alterar?id=" + id)
            .then(function (response) {
                return response.json();
            })
            .then(dados => {
                document.getElementById("idLivro").value = dados.livro.id;
                document.getElementById("nome").value = dados.livro.nome;
                document.getElementById("qtd").value = dados.livro.qtd;
                document.getElementById("editoraId").value = dados.livro.editora.id;
                indexLivro.montarAutores(dados.livro.autor);
            }).catch(function (error) {
                console.error(error);
            });
    },

    montarAutores: (autor) => {
        var select = document.querySelector("[data-select]");
        autor.forEach(item => {
            for (let i = 0; i < select.options.length; i++) {
                    select.options[i].removeAttribute("selected", "selected");
            }
        });
        autor.forEach(item => {
            for (let i = 0; i < select.options.length; i++) {
                if (select.options[i].value == item.id)
                    select.options[i].setAttribute("selected", "selected");
            }
        });
    },

    enviar: () => {
        document.getElementById("gravando").innerHTML = "Gravando... Aguarde!";
        var select = document.getElementById("autorId");
        var autoresId = [];
        for (let i = 0; i < select.options.length; i++) {
            if (select.options[i].selected)
                autoresId.push(select.options[i].value);
        }
        let dados = {
            nome: document.getElementById("nome").value,    
            editora: document.getElementById("editoraId").value,
            autor: autoresId,
            qtd: document.getElementById("qtd").value,
            idLivro: document.getElementById("idLivro").value
        }
        HTTPClient.post("/Livro/Gravar", dados)
            .then(result => {
                return result.json();
            })
            .then(dados => {
                if (dados.msg == "Falha ao Gravar Livro!") {
                    document.getElementById("gravou").innerHTML = "<div class='alert alert-danger alert-dismissible' role='alert'><button type='button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>×</span></button><i class='fa fa-times-circle'></i> " + dados.msg + "!</div>";
                }
                else if (dados.msg == "Preencha Todos os Campos!!!") {
                    document.getElementById("gravou").innerHTML = "<div class='alert alert-danger alert-dismissible' role='alert'><button type='button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>×</span></button><i class='fa fa-times-circle'></i> " + dados.msg + "!</div>";
                }
                else
                    document.getElementById("gravou").innerHTML = "<div class='alert alert-success alert-dismissible' role='alert'><button type='button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>×</span></button><i class='fa fa-check-circle'></i> " + dados.msg + "</div>";
                indexLivro.pesquisar();
            })
            .catch(() => {
                console.log("Falha ao Gravar");
            });
        document.getElementById("gravando").innerHTML = "";
    },

}

document.addEventListener("DOMContentLoaded", () => {
    indexLivro.init();
});