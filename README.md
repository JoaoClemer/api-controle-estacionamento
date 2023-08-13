<h1>API Parking Control</h1>

Para geração do token de autenticação, segue os usuários já cadastrados:

Esse tem a permisão de Admin, que autoriza ele:<br>
- Atualizar dados dos usuários (PUT)<br>
- Deletar usuários(DELETE)<br>
- Ativar ou desativar usuários(PATCH)
```
{
  "username": "joaoclemer",
  "passwordHash": "123"
}
```
Esse tem a permisão de VehicleRegister, que autoriza ele:<br>
- Registrar novos carros no estacionamento (POST)
```
{
  "username": "clemercarvalho",
  "passwordHash": "123"
}
```
Gere o token através do End Point Account:
![Account](https://github.com/JoaoClemer/api-controle-estacionamento/assets/56324622/c904d9c6-6b74-415f-adf8-cb12c75a31f2)
![Token](https://github.com/JoaoClemer/api-controle-estacionamento/assets/56324622/d4b7e644-b1b3-4d3e-8ec8-01e28e7ecfa4)

Utilize o token no botão "Authorize":
<br>
![Authorize](https://github.com/JoaoClemer/api-controle-estacionamento/assets/56324622/53eb3932-de37-4504-9eb1-591a35e17648)

Exemplo: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1lIjoiam9hb2NsZW1lciIsInJvbGUiOiJBZG1pbiIsIm5iZiI6MTY5MTk2NTcwOCwiZXhwIjoxNjkxOTcyOTA4LCJpYXQiOjE2OTE5NjU3MDh9.YF6Gcgv_sG94Lx9QrxHTPKeNfHOHuDOy9NNTz36x548

<h2>Diagrama UML do projeto</h2>

![UML](https://github.com/JoaoClemer/api-controle-estacionamento/assets/56324622/281a6b03-c1a6-4f0d-8efb-44c3cc0dcd43)
