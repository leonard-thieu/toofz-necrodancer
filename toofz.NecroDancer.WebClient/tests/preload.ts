const g = global as any;
g.jQuery = require('jquery');
g.$ = jQuery;

require('../node_modules/bootstrap-3-typeahead/bootstrap3-typeahead');

import * as chai from 'chai';
import * as chaiAsPromised from 'chai-as-promised';
import * as chaiInterface from 'chai-interface';
import * as sinonChai from 'sinon-chai';

g.should = chai.should();

chai.use(chaiAsPromised);
chai.use(chaiInterface);
chai.use(sinonChai);