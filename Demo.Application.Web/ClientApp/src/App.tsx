import * as React from 'react';
import { Route } from 'react-router';
import Layout from './components/Layout';
import Home from './components/Home';
import Counter from './components/Counter';
import FetchData from './components/FetchData';

import './custom.css'
import FetchEmployee from './components/FetchEmployee';
import AddEmployee from './components/AddEmployee';


export default () => (
    <Layout>
        <Route exact path='/' component={Home} />
        <Route path='/counter' component={Counter} />
        <Route path='/fetch-data/:startDateIndex?' component={FetchData} />
        <Route path='/employee/:startDateIndex?' component={FetchEmployee} />
        <Route path='/editemployee/edit/:empid?' component={AddEmployee} />
        <Route path='/addemployee' component={AddEmployee} />


    </Layout>
);
