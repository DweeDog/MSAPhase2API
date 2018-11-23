import {AppBar, IconButton, Toolbar, Typography} from '@material-ui/core/';

import MenuIcon from '@material-ui/icons/Menu';

import * as React from 'react';
// import { Nav,  NavItem } from 'react-bootstrap';
// import { IndexLinkContainer } from "react-router-bootstrap";


export const Header: React.StatelessComponent<{}> = () => {    

    return (
            <AppBar position="static">
                <Toolbar>
                    <IconButton  aria-label="Menu" color="inherit">
                        <MenuIcon aria-haspopup="true"/>
                    </IconButton>

                    <Typography variant="h6" color="inherit">
                        MyContactBook
                    </Typography>
                </Toolbar>

            </AppBar>
           
    );
}

export default Header;