# DatingApplication

### Jak używać endpointu `/matches`

#### Parametry
+ `user_id` - definiuje, dla kogo mają być wyszukane potencjalne pary
+ `num_matches` - definiuje ile najlepszych par ma być zwróconych spośród wszystkich

### Wartość zwracana

Endpoint zwraca listę JSON'ów, z czego każdy zawiera trzy pola:
+ `score` - compatybilność z zakresu <0, 1>
+ `target_user_id` - id osoby, z którą parowany był użytkownik
+ `user_id` - id przekazane w parametrach wejściowych

### Przykład

`localhost:5000/matches?user_id=1&num_matches=3`

daje:

`[{"score":0.9227024042076089,"target_user_id":6,"user_id":1},{"score":0.9204788825745897,"target_user_id":4,"user_id":1},{"score":0.8993866036393809,"target_user_id":3,"user_id":1}]`
