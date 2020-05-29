import React from 'react';

import Card from '../card.jsx';
import Loader from '../loader.jsx';
import APIComponent from './apiComponent.jsx';

class ValueView extends APIComponent{

    load(){
        this.loadData('value');
    }

    render(){
        let value = <Loader/>;
        if(this.state.load){
            value = (
                <span>
                    <h5>Wartość dla x = {this.state.x}</h5>
                    <p className="s-text--indent">{this.props.equation.replace(/\{(.*?)\}$/g,'')} = {this.state.value}</p>
                </span>
            );
        }
        return(
            <Card type="warning">
                {value}
            </Card>
        );
    }

}

export default ValueView;