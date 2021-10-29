let indexLeitor = {
    carregar: () => {

        HTTPClient.get("/Leitor/Listar")
            .then(function (response) {
                return response.json();
            })
            .then(dados => {
                var table = "";
                var date;
                dados.forEach(item => {
                    date = new Date(`${item.dataNasc}`).toLocaleDateString().substr(0, 10);
                    table += `<tr>
                        <td>${item.id}</td>
                        <td>${item.nome}</td>
                        <td>${item.cpf}</td>
                        <td>${item.endereco}</td>
                        <td>${date}</td>
                        <td onclick='indexLeitor.apagar(${item.id})'>X</td>
                        <td onclick='indexLeitor.alterar(${item.id})'>▓</td>
                              </tr>`;
                });

                document.getElementById("corpo-tabela").innerHTML = table;
            }).catch(function (error) {
                console.error(error);
            });
    },

    pesquisar: () => {
        var cpf = document.getElementById("pesquisaLeitor").value;
        if (cpf.length < 14) {
            alert("Digite o CPF Completo");
            return;
        }
        HTTPClient.get("/Leitor/Pesquisar?cpf=" + cpf)
            .then(function (response) {
                return response.json();
            })
            .then(dado => {
                if (dado != null) {
                    var table = "";
                    var date;
                    debugger
                    date = new Date(`${dado.dataNasc}`).toLocaleDateString().substr(0, 10);
                    table += `<tr>
                    <td>${dado.id}</td>
                    <td>${dado.nome}</td>
                    <td>${dado.cpf}</td>
                    <td>${dado.endereco}</td>
                    <td>${date}</td>
                    <td onclick='indexLeitor.apagar(${dado.id})'>X</td>
                    <td onclick='indexLeitor.alterar(${dado.id})'>▓</td>
                            </tr>`;;

                    document.getElementById("corpo-tabela").innerHTML = table;
                    document.getElementById("pesquisaLeitor").value = "";
                }
                else
                    alert("CPF Nao Encontrado!");
            }).catch(function (error) {
                console.error(error);
            });
    },

    enviar: () => {

        let dados = {
            id: document.getElementById("valorId").value,
            nome: document.getElementById("nomeLeitor").value,
            cpf: document.getElementById("cpfLeitor").value,
            dataNasc: document.getElementById("dataLeitor").value,
            endereco: document.getElementById("enderecoLeitor").value
        }

        HTTPClient.post("/Leitor/Gravar", dados)
            .then(result => {
                return result.json();
            })
            .then(dados => {
                if (dados.msg == "Falha ao Gravar Leitor!") {
                    document.getElementById("gravou").innerHTML = "<div class='alert alert-danger alert-dismissible' role='alert'><button type='button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>×</span></button><i class='fa fa-times-circle'></i> " + dados.msg + "!</div>";
                }
                else if (dados.msg == "Preencha Todos os Campos") {
                    document.getElementById("gravou").innerHTML = "<div class='alert alert-danger alert-dismissible' role='alert'><button type='button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>×</span></button><i class='fa fa-times-circle'></i> " + dados.msg + "!</div>";
                }
                else
                    document.getElementById("gravou").innerHTML = "<div class='alert alert-success alert-dismissible' role='alert'><button type='button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>×</span></button><i class='fa fa-check-circle'></i> " + dados.msg + "</div>";
                indexLeitor.limparForm();
                indexLeitor.carregar();
            })
            .catch(() => {
                console.log("Falha ao Gravar");
            }); 
    },

    apagar: (id) => {
        HTTPClient.delete("/Leitor/Deletar?id=" + id)
            .then(function (response) {
                return response.json();
            })
            .then(dados => {
                indexLeitor.carregar();
                alert(dados.msg);
            }).catch(function (error) {
                console.error(error);
            });
        indexLeitor.carregar();
    },

    alterar: (id) => {
        HTTPClient.put("/Leitor/Alterar?id=" + id)
            .then(function (response) {
                return response.json();
            })
            .then(dados => {
                var date = new Date(`${dados.dataNasc}`).toLocaleDateString('en-US').substr(0, 10);
                var dataformatada = indexLeitor.formatarData(date);
                document.getElementById("valorId").value = dados.id;
                document.getElementById("nomeLeitor").value = dados.nome;
                document.getElementById("cpfLeitor").value = dados.cpf;
                document.getElementById("dataLeitor").value = dataformatada;
                document.getElementById("enderecoLeitor").value = dados.endereco;
            }).catch(function (error) {
                console.error(error);
            });
    },

    formatarData: (data) =>
    {
        var mes = data.split("/")[0];
        var dia = data.split("/")[1];
        var ano = data.split("/")[2];

        return ano + '-' + ("0" + mes).slice(-2) + '-' + ("0" + dia).slice(-2);
    },

    limparForm: () => {
        document.getElementById("valorId").value = 0;
        document.getElementById("nomeLeitor").value = "";
        document.getElementById("cpfLeitor").value = "";
        document.getElementById("dataLeitor").value = "";
        document.getElementById("enderecoLeitor").value = "";
    }

}

document.addEventListener("DOMContentLoaded", () => {
    indexLeitor.carregar();
});



