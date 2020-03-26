# Reverse polish notation C#

## Opis funkcji i właściwości:

##### Obsługiwane zmienne matematyczne:
* pi
* e

!! W podanej formule nie są rozróżniane wielkości znaków !!

!! Nazwa zmiennej może składać się jedynie ze znaków alfabetu a-z !!

```csharp
try{
    RPN myMathEquation = new RPN("-sin(2)+pi-e"); //Inicjacja obiektu
}catch(Exception e){ //Domyślnie wszyskie błędy będą zwracane poprzez wiadomośc w wyjątku typu RPNException
    //np. gdy podamy błędną formułę.
    Console.WriteLine(e.Message);
}
//Obliczenie wartości:
double result  = myMathEquation.getValue(); //Pojedyncza wartość
myMathEquation.getValues(); //Przedział zmiennej, aktualnie jedynie działa dla zmiennej x i wypisuje w konsoli
//Metoda zwracająca poprawnośc formuły domyślnie wywoływana w konstruktorze
bool formulaCheck = myMathEquation.isInfixNotation(); 
//Zwrócenie tokenów
string[] infixTokens = myMathEquation.getTokens(); //notacja infix
string[] postfixTokens = myMathEquation.getPosfixSyntax(); //odwrotna notacja polska
//Sprawdzenie czy dany token jest funkcją
bool tokenCheck = RPN.isFunction("sin"); //true
//Sprawdzenie czy dany token jest operatorem
bool tokenCheck = RPN.isFunction("*"); //true

//Podstawianie pod zmienną:
RPN myMathEquation = new RPN("-sin(2*x) {x:2.32}"); //Z użyciem konstruktora
myMathEquation.setVariable("x",2.32); //Z użyciem funkcji
myMathEquation.setVariable("x",new EquationVariable(2.32)); //Z użyciem metody
//Inne z użyciem metody
string varName;
double varValue = EquationVariable.getFromString("x:2.32",out varName);
myMathEquation.setVariable(varName,varValue);
//Inne z użyciem metody
myMathEquation.setVariable("x",EquationVariable.getFromString("x:2.32"));

//Podstawianie pod parę zmiennych
RPN myMathEquation = new RPN("-sin(2*x-y)*z {x:2.32,y:5.32,}"); //Z użyciem konstruktora
//Reszta jak w przypadku pojedynczej zmiennej

//Ustawianie przedziałów:
RPN myMathEquation = new RPN("-sin(2*x) {x:(from,to,n-steps)}"); //Z użyciem konstruktora
myMathEquation.setVariable("x",new EquationVariable(from,to,step)); //Z użyciem metody
//Inne z użyciem metody
string varName;
double varValue = EquationVariable.getFromString("x:(from,to,n-steps)",out varName);
myMathEquation.setVariable(varName,varValue);
//Inne z użyciem metody
myMathEquation.setVariable("x",EquationVariable.getFromString("x:(from,to,n-steps)"));

//Wywołanie zdarzenia dla nieznanej zmiennej przykład
myMathEquation.onVariableAsk+=((varName)=>{
    Console.WriteLine("Put \"{0}\" value:",varName);
    double varValue = Double.Parse(Console.ReadLine());
    return varValue;
});

//Pobranie par zmiennych w postaci {varName:varValue,varName2:varValue2...}
string[] paris = EquationVariable.getPairs("{varName:varValue,varName2:varValue2}");
foreach(string var in pairs){
    string varName;
    double varValue = EquationVariable.getFromString(var,out varName);
    Console.WriteLine("{0}=>{1}",varName,varValue);
}

//Pobranie wartości zmiennej
EquationVariable var = EquationVariable.getFromString("x:2.56");
double value = var.getValue();

//Pobranie kolejnej wartości z przedziału
EquationVariable interval = EquationVariable.getFromString("x:(0,20,5)");
for(int i=0;i<interval.steps;i++){
    Console.WriteLine(interval.getValue());
    interval.next(); //Ustawia kolejny krok gdy przekroczy liczy od początku
}
interval.reset(); //Ustawia licznik na krok 0

//Zamiana string na double w postaci z kropką
try{
    double myDoubleNumber = RPN.parseDouble("2.32");
}catch(RPNException e){
    Console.WriteLine(e.Message);
}
```

## Kiedy jest rzucany warunek RPNException:
* Gdy podamy błędną formułę.
* Gdy podamy będą formułę zmiennej/przedziału.
* Gdy podamy wartość liczbową w będnej postaci.
* Gdy nie będziemy mieć przypisanego zdarzenia do nieznanej zmiennej.
* Gdy wystąpi nieznany błąd obliczeń, spowodowany błędem algorytmu liczącego.