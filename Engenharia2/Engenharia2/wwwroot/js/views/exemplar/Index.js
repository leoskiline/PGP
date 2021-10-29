let indexExemplar = {
    carregar: () => {

        HTTPClient.get("/Exemplar/Listar")
            .then(function (response) {
                return response.json();
            })
            .then(dados => {
                var table = "";
                dados.forEach(item => {
                    table += `<tr>
                        <td>${item.id}</td>
                        <td>${item.livro.nome}</td>
                        <td>${item.posicao.setor}</td>
                        <td>${item.posicao.prateleira}</td>
                        <td onclick='indexExemplar.apagar(${item.id})'>X</td>
                        <td onclick='indexExemplar.alterar(${item.id})'>▓</td>
                              </tr>`;
                });

                document.getElementById("corpo-tabela").innerHTML = table;
            }).catch(function (error) {
                console.error(error);
            });
    },

    pesquisar: () => {
        var nome = document.getElementById("pesquisaExemplar").value;
        debugger;
        HTTPClient.get("/Exemplar/Pesquisar?nome=" + nome)
            .then(function (response) {
                return response.json();
            })
            .then(dados => {
                var table = "";
                dados.forEach(item => {
                    table += `<tr>
                        <td>${item.id}</td>
                        <td>${item.livro.nome}</td>
                        <td>${item.posicao.setor}</td>
                        <td>${item.posicao.prateleira}</td>
                        <td onclick='indexExemplar.apagar(${item.id})'>X</td>
                        <td onclick='indexExemplar.alterar(${item.id})'>▓</td>
                              </tr>`;
                });

                document.getElementById("corpo-tabela").innerHTML = table;
            }).catch(function (error) {
                console.error(error);
            });
    },

    apagar: (id) => {
        HTTPClient.delete("/Exemplar/Deletar?id=" + id)
            .then(function (response) {
                return response.json();
            })
            .then(dados => {
                indexExemplar.carregar();
                alert(dados.msg);
            }).catch(function (error) {
                console.error(error);
            });
        indexExemplar.carregar();
    },

    alterar: (id) => {
        HTTPClient.put("/Exemplar/Alterar?id=" + id)
            .then(function (response) {
                return response.json();
            })
            .then(dados => {
                debugger;
                document.getElementById("exemplarId").value = dados.exemplar.id;
                document.getElementById("livroId").value = dados.exemplar.livro.id;
                document.getElementById("setorPos").value = dados.exemplar.posicao.setor;
                document.getElementById("prateleiraPos").value = dados.exemplar.posicao.prateleira;
                document.getElementById("posicaoId").value = dados.exemplar.posicao.id;
                indexExemplar.carregar();
            }).catch(function (error) {
                console.error(error);
            });
    },

    limparForm: () => {
        document.getElementById("livroId").value = 0;
        document.getElementById("setorPos").value = "";
        document.getElementById("prateleiraPos").value = "";
    },

    enviar: () => {

        let dados = {
            exemplarId: document.getElementById("exemplarId").value,
            posicaoId: document.getElementById("posicaoId").value,
            livroId: document.getElementById("livroId").value,
            setor: document.getElementById("setorPos").value,
            prateleira: document.getElementById("prateleiraPos").value
        }

        HTTPClient.post("/Exemplar/Gravar", dados)
            .then(result => {
                return result.json();
            })
            .then(dados => {
                if (dados.msg == "Falha ao Gravar Exemplar!") {
                    document.getElementById("gravou").innerHTML = "<div class='alert alert-danger alert-dismissible' role='alert'><button type='button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>×</span></button><i class='fa fa-times-circle'></i> " + dados.msg + "!</div>";
                }
                else if (dados.msg == "Preencha os Campos!!!") {
                    document.getElementById("gravou").innerHTML = "<div class='alert alert-danger alert-dismissible' role='alert'><button type='button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>×</span></button><i class='fa fa-times-circle'></i> " + dados.msg + "!</div>";
                }
                else if (dados.msg == "Posicao Nao Existe!") {
                    document.getElementById("gravou").innerHTML = "<div class='alert alert-info alert-dismissible' role='alert'><button type='button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>×</span></button><i class='fa fa-times-circle'></i> " + dados.msg + "!</div>";
                }
                else
                    document.getElementById("gravou").innerHTML = "<div class='alert alert-success alert-dismissible' role='alert'><button type='button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>×</span></button><i class='fa fa-check-circle'></i> " + dados.msg + "</div>";
                indexExemplar.carregar();
            })
            .catch(() => {
                console.log("Falha ao Gravar");
            });


    },

}

document.addEventListener("DOMContentLoaded", () => {
    indexExemplar.carregar();
});




