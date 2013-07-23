use Test::More;
use Test::Exception;
use Regexp::Common;

use_ok('Poker::Dealer');
use_ok('Poker::Card');

new_ok('Poker::Dealer');

subtest 'isFullHouse' => sub{
   ok(! Poker::Dealer::isFullHouse());
   ok(! Poker::Dealer::isFullHouse(Poker::Card->new('2','c'),Poker::Card->new('2','s'),Poker::Card->new('2','d'),Poker::Card->new('2','h'),Poker::Card->new('3','d')));
   ok(! Poker::Dealer::isFullHouse(Poker::Card->new('2','c'),Poker::Card->new('2','s'),Poker::Card->new('2','d'),Poker::Card->new('4','c'),Poker::Card->new('3','d')));
   ok(Poker::Dealer::isFullHouse(Poker::Card->new('2','c'),Poker::Card->new('2','s'),Poker::Card->new('2','d'),Poker::Card->new('3','c'),Poker::Card->new('3','d')));
};

subtest 'isRoyalFlush' => sub{
   ok(1);
};

done_testing();
