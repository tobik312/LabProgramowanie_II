import React from 'react';

import Card from './card.jsx';

function Instruction(){
    return(
        <Card>
            <h1>Instrukcja:</h1>
            <p className="padding s-text--justify">
                W celu obliczenia wartości wpisz równanie ze zmienną x.
                <br/>
                <br/>
                Przykład: sin(3*x)
                <br/>
                Domyślnie za wartość x jest uznawana liczba 0.
            </p>
            <p className="padding s-text--justify">
                W celu ustawienia swojej dowolnej zmiennej po równaniu należy zapisać wartość w danej notacji: {'{nazwaZmiennej:wartośćZmiennej}'}
                <br/>
                <br/>
                Przykład: {'{x:3,y:2.3}'}
            </p>
            <p className="padding s-text--justify">
                Wybierając "pokaż tokeny", zostanie ci wyświetlone dane równanie w notacji infix i postfix.
            </p>
            <p className="padding s-text--justify">
                Po obliczeniu wartośći zostanie również zaprezentowany wykres funkcji, którego parametry można dowolnie modyfikować.
            </p>
            <h5 className="padding">Obsługiwane zmienne matematyczne:</h5>
            <ul>
                <li>pi</li>
                <li>e</li>
            </ul>
            <h5 className="padding">Uwagi:</h5>
            <ul>
                <li>W podanej formule nie są rozróżniane wielkości znaków.</li>
                <li>Nazwa zmiennej może składać się jedynie ze znaków alfabetu a-z.</li>
            </ul>
        </Card>
    );
}

export default Instruction