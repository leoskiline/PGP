let indexEditora = {

    carregar: () => {
        
        HTTPClient.get("/Editora/Listar")
            .then(function (response) {
                return response.json();
            })
            .then(dados => {
                var table = "";
                dados.forEach(item => {
                    table += `<tr>
                        <td>${item.id}</td>
                        <td>${item.nome}</td>

                        <td>${item.descricao}</td>
                        <td>${item.telefone}</td>
                        <td onclick='indexEditora.apagar(${item.id})'>X</td>
                        <td onclick='indexEditora.alterar(${item.id})'>▓</td>
                              </tr>`;
                });

                document.getElementById("corpo-tabela").innerHTML = table;
            }).catch(function (error) {
                console.error(error);
            });
    },

    pesquisar: () => {
        var nome = document.getElementById("pesquisaEditora").value;
        HTTPClient.get("/Editora/Pesquisar?nome=" + nome)
            .then(function (response) {
                return response.json();
            })
            .then(dados => {
                var table = "";
                dados.forEach(item => {
                    debugger
                    table += `<tr>
                        <td>${item.id}</td>
                        <td>${item.nome}</td>
                        <td>${item.descricao}</td>
                        <td>${item.telefone}</td>
                        <td onclick='indexEditora.apagar(${item.id})'>X</td>
                        <td onclick='indexEditora.alterar(${item.id})'>▓</td>
                              </tr>`;
                });
                
                document.getElementById("corpo-tabela").innerHTML = table;
                document.getElementById("pesquisaEditora").value = "";
            }).catch(function (error) {
                console.error(error);
            });
    },

    enviar: () => {

        let dados = {
            id: document.getElementById("id").innerHTML,
            nome: document.getElementById("nome").value,
            descricao: document.getElementById("descricao").value,
            telefone: document.getElementById("telefone").value
        }
        HTTPClient.post("/Editora/Gravar", dados)
            .then(result => {
                
                return result.json();
            })
            .then(dados => {
                if (dados.msg == "Falha ao Gravar Editora!") {
                    document.getElementById("gravou").innerHTML = "<div class='alert alert-danger alert-dismissible' role='alert'><button type='button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>×</span></button><i class='fa fa-times-circle'></i> "+dados.msg+"!</div>";
                }
                else if (dados.msg == "Falha ao Alterar Editora!") {
                    document.getElementById("gravou").innerHTML = "<div class='alert alert-danger alert-dismissible' role='alert'><button type='button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>×</span></button><i class='fa fa-times-circle'></i> " + dados.msg + "!</div>";
                }
                else if (dados.msg == "Preencha Todos os Campos"){
                    document.getElementById("gravou").innerHTML = "<div class='alert alert-danger alert-dismissible' role='alert'><button type='button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>×</span></button><i class='fa fa-times-circle'></i> " + dados.msg + "!</div>";
                }
                else
                    document.getElementById("gravou").innerHTML = "<div class='alert alert-success alert-dismissible' role='alert'><button type='button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>×</span></button><i class='fa fa-check-circle'></i> " + dados.msg + "</div>";
                indexEditora.carregar();
                indexEditora.limparForm();
            })
            .catch(() => {
                console.log("Falha ao Gravar");
            });
    },

    apagar: (id) => {
        HTTPClient.delete("/Editora/Deletar?id=" + id)
            .then(function (response) {
                return response.json();
            })
            .then(dados => {
                indexEditora.carregar();
                alert(dados.msg);
            }).catch(function (error) {
                console.error(error);
            });
    },

    alterar: (id) => {
        var fdados = document.getElementById("editora-fdados");

        HTTPClient.put("/Editora/Alterar?id=" + id, fdados)
            .then(function (response) {
                return response.json();
            })
            .then(dados => {
                document.getElementById("id").innerHTML = dados.id;
                document.getElementById("nome").value = dados.nome;
                document.getElementById("descricao").value = dados.descricao;
                document.getElementById("telefone").value = dados.telefone;
                
                //fdados.nome.value = dados.nome;
                //fdados.descricao.value = dados.descricao;
                //fdados.telefone.value = dados.telefone;

            }).catch(function (error) {
                console.error(error);
            });
    },

    limparForm: () => {
        document.getElementById("id").innerHTML = "0"
        var fdados = document.getElementById("editora-fdados");
        debugger
        fdados.nome.value = "";
        fdados.descricao.value = "";
        fdados.telefone.value = "";
        
    }
}


document.addEventListener("DOMContentLoaded", () => {
    indexEditora.carregar();
});

