import React, { Suspense, lazy, Component } from 'react';
import axios from "axios";
import Header from './components/header';
import "./App.css";

const Movies = lazy(() => import('./components/movies'));

    class App extends Component {
        state = { movies: []};


        constructor(props) {
            super(props);
            this.state = { movies: [], loading: true };
        }

        componentDidMount() {
            axios
                .get('api/Movie/MovieList')
                .then(data => {
                    this.setState({ movies: data.data, loading: false });
            });
    }

    render() {
        const loadingImg = <div className="movie-img">
            <img alt="loading" src="https://media.giphy.com/media/y1ZBcOGOOtlpC/200.gif" />
        </div>

        const contents = this.state.loading
            ? <p><em>Loading...</em></p>
            : this.state.movies.map(e => {
            return (
                <Suspense key={e.id} fallback={loadingImg}>
                    <Movies
                        poster={e.poster}
                        title={e.title}
                        id={e.id}
                        price={e.price}
                        date={e.year}
                        provider={e.provider}
                    />
                </Suspense>
            );
        });

        return (
            <div className="app">
                <Header />
                <div className="movies">
                    {contents}
                </div>
            </div>

        );
    }
}

export default App;