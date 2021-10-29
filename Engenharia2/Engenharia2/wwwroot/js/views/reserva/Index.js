let indexReserva = {

    carregar: () => {

        HTTPClient.get("/Reserva/Listar")
            .then(function (response) {
                return response.json();
            })
            .then(dados => {
                var table = "";
                dados.forEach(item => {
                    table += `<tr>
                        <td>${item.id}</td>
                        <td>${item.leitor.cpf}</td>
                        <td>${item.livro.nome}</td>
                        <td>${item.status}</td>
                        <td onclick='indexReserva.cancelar(${item.id})'>X</td>
                              </tr>`;
                });

                document.getElementById("corpo-tabela").innerHTML = table;
            }).catch(function (error) {
                console.error(error);
            });
    },

    pesquisar: () => {
        var cpf = document.getElementById("pesquisaLeitor").value;
        var status = document.getElementById("status-reserva").value;
        HTTPClient.get("/Reserva/Pesquisar?cpf=" + cpf + "&status=" + status)
            .then(function (response) {
                return response.json();
            })
            .then(dados => {
                var table = "";
                dados.forEach(item => {
                    table += `<tr>
                        <td>${item.id}</td>
                        <td>${item.leitor.cpf}</td>
                        <td>${item.livro.nome}</td>
                        <td>${item.status}</td>
                        <td onclick='indexReserva.cancelar(${item.id})'>X</td>
                              </tr>`;
                });

                document.getElementById("corpo-tabela").innerHTML = table;
                document.getElementById("pesquisaLeitor").value = "";
            }).catch(function (error) {
                console.error(error);
            });
    },

    enviar: () => {

        let dados = {
            cpf: document.getElementById("cpf").value,
            livro: document.getElementById("livroId").value
        }
        debugger
        HTTPClient.post("/Reserva/Gravar", dados)
            .then(result => {
                return result.json();
            })
            .then(dados => {
                if (dados.msg == "Falha ao Gravar Reserva!") {
                    document.getElementById("gravou").innerHTML = "<div class='alert alert-danger alert-dismissible' role='alert'><button type='button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>×</span></button><i class='fa fa-times-circle'></i> " + dados.msg + "!</div>";
                }
                else if (dados.msg == "Preencha Todos os Campos") {
                    document.getElementById("gravou").innerHTML = "<div class='alert alert-danger alert-dismissible' role='alert'><button type='button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>×</span></button><i class='fa fa-times-circle'></i> " + dados.msg + "!</div>";
                }
                else
                    document.getElementById("gravou").innerHTML = "<div class='alert alert-success alert-dismissible' role='alert'><button type='button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>×</span></button><i class='fa fa-check-circle'></i> " + dados.msg + "</div>";

                indexReserva.carregar();
                indexReserva.limparForm();
            })
            .catch(() => {
                console.log("Falha ao Gravar");
            });
    },

    cancelar: (id) => {
        HTTPClient.put("/Reserva/Cancelar?id=" + id)
            .then(function (response) {
                return response.json();
            })
            .then(dados => {
                indexReserva.carregar();
                alert(dados.msg);
            }).catch(function (error) {
                console.error(error);
            });
    },

    limparForm: () => {
        var fdados = document.getElementById("reserva-fdados");
        fdados.cpf.value = "";
        fdados.livroId.options[0];
    }

}



document.addEventListener("DOMContentLoaded", () => {
    indexReserva.carregar();
});