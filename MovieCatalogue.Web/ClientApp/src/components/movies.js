import React from 'react';
import LazyLoad from 'react-lazyload';

import './movies.css'

export default (props) => {
    return (
        <ul className="movie">

            <li className="movie-item">

                <LazyLoad height={200} >
                    <img className="movie-img" src={props.poster}/>
                </LazyLoad>
            </li>

            <li className="title movie-item">
                {props.title.slice(0, 50)}</li>
            <li className="date movie-item">Released: {props.date}</li>
            <li className="price movie-item">Price: {props.price}</li>
            <li className="date movie-item">Cheapest From: {props.provider}</li>
        </ul>
    )

}