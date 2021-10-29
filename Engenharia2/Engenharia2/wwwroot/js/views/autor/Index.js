let indexAutor = {

    carregar: () => {

        HTTPClient.get("/Autor/Listar")
            .then(function (response) {
                return response.json();
            })
            .then(dados => {
                var table = "";
                dados.forEach(item => {
                    table += `<tr>
                        <td>${item.id}</td>
                        <td>${item.nome}</td>
                        <td onclick='indexAutor.apagar(${item.id})'>X</td>
                        <td onclick='indexAutor.alterar(${item.id})'>▓</td>
                              </tr>`;
                });

                document.getElementById("corpo-tabela").innerHTML = table;
            }).catch(function (error) {
                console.error(error);
            });
    },

    pesquisar: () => {
        var nome = document.getElementById("pesquisaAutor").value;
        HTTPClient.get("/Autor/Pesquisar?nome=" + nome)
            .then(function (response) {
                return response.json();
            })
            .then(dados => {
                var table = "";
                dados.forEach(item => {
                    table += `<tr>
                        <td>${item.id}</td>
                        <td>${item.nome}</td>
                        <td onclick='indexAutor.apagar(${item.id})'>X</td>
                        <td onclick='indexAutor.alterar(${item.id})'>▓</td>
                              </tr>`;
                });

                document.getElementById("corpo-tabela").innerHTML = table;
                document.getElementById("pesquisaAutor").value = "";
            }).catch(function (error) {
                console.error(error);
            });
    },

    enviar: () => {

        let dados = {
            id: document.getElementById("id").innerHTML,
            nome: document.getElementById("nome").value,
        }
        debugger
        HTTPClient.post("/Autor/Gravar", dados)
            .then(result => {
                return result.json();
            })
            .then(dados => {
                if (dados.msg == "Falha ao Gravar Autor!") {
                    document.getElementById("gravou").innerHTML = "<div class='alert alert-danger alert-dismissible' role='alert'><button type='button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>×</span></button><i class='fa fa-times-circle'></i> " + dados.msg + "!</div>";
                }
                else if (dados.msg == "Falha ao Alterar Autor!") {
                    document.getElementById("gravou").innerHTML = "<div class='alert alert-danger alert-dismissible' role='alert'><button type='button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>×</span></button><i class='fa fa-times-circle'></i> " + dados.msg + "!</div>";
                }
                else if (dados.msg == "Preencha Todos os Campos") {
                    document.getElementById("gravou").innerHTML = "<div class='alert alert-danger alert-dismissible' role='alert'><button type='button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>×</span></button><i class='fa fa-times-circle'></i> " + dados.msg + "!</div>";
                }
                else
                    document.getElementById("gravou").innerHTML = "<div class='alert alert-success alert-dismissible' role='alert'><button type='button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>×</span></button><i class='fa fa-check-circle'></i> " + dados.msg + "</div>";
                indexAutor.carregar();
                indexAutor.limparForm();
            })
            .catch(() => {
                console.log("Falha ao Gravar");
            });
    },

    apagar: (id) => {
        HTTPClient.delete("/Autor/Deletar?id=" + id)
            .then(function (response) {
                return response.json();
            })
            .then(dados => {
                indexAutor.carregar();
                alert(dados.msg);
            }).catch(function (error) {
                console.error(error);
            });
    },

    alterar: (id) => {
        var fdados = document.getElementById("autor-fdados");

        HTTPClient.put("/Autor/Alterar?id=" + id, fdados)
            .then(function (response) {
                return response.json();
            })
            .then(dados => {
                document.getElementById("id").innerHTML = dados.id;
                document.getElementById("nome").value = dados.nome;
            }).catch(function (error) {
                console.error(error);
            });
    },

    limparForm: () => {
        document.getElementById("id").innerHTML = "0"
        var fdados = document.getElementById("autor-fdados");
        fdados.nome.value = "";
    }

}



document.addEventListener("DOMContentLoaded", () => {
    indexAutor.carregar();
});