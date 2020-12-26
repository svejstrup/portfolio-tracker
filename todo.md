Azure static web app
React frontend
C# azure functions api
Table storage database

# Web
- En side - øverst graf over udviklingen, nederst tabel med beholdning
- Mulighed for at skifte view i tabellen. Nuværende beholdning og afsluttede handler
- nuværende beholdning - Navn, dagens stigning, total stigning, beholdning, kurs, urealiseret gevinst
- Afsluttede handler - Navn, købsdato, salgsdato, købsbeløb, salgsbeløb, procent fortjeneste, total fortjeneste
- knap til upload af csv

# API
- Service manager, der snakker med Yahoo API
- Endpoint der henter data til tabeller - to lister (nuværende og afsluttede), findes ved dels at gå transaktioner igennem og lave kald til Yahoo om nuværende kurser
- Endpoint der henter data til graf - liste af kurser
- Endpoint til at importere csv - parse og opdater transaktioner + slå historiske lukkekurser op og opdater lukkekurser i perioden den har været ejet
- Timer funktion én gang om dagen opdatere lukkekurser på aktuel beholdning

# Database
- transaktioner
- lukkekurser aktuel beholdning, en række pr aktie pr dag med kode, dato, kurs, valuta samt vekselkurs
