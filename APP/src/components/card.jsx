import React from 'react';

function Card(props){
    return (
        <div className={`s-card--${props.type ? props.type : 'primary'}`}>
			<div className="s-card__content">
                {props.children}
            </div>
        </div>
    );
}

export default Card;