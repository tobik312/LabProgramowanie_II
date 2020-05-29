import React from 'react';

import Card from '../card.jsx';
import Loader from '../loader.jsx';
import APIComponent from './apiComponent.jsx';

class TokensView extends APIComponent{

    load(){
        this.loadData('tokens');
    }

    render(){
        let tokens = <Loader/>
        if(this.state.load){
            tokens = (
                <span>
                    <h5>Notacja postfix</h5>
                    <p className="s-text--indent">
                        {this.state.tokens.postfix.join(' ')}
                    </p>
                    <h5>Notacja infix</h5>
                    <p className="s-text--indent">
                        {this.state.tokens.infix.join(' ')}
                    </p>
                </span>
            );
        }
        return(
            <Card type="danger">
                {tokens}
            </Card>
        );
    }

}

export default TokensView;