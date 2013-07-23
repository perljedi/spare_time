use Test::More;

use_ok('Poker::Card');
can_ok(Poker::Card, 'new');
my $card = new_ok('Poker::Card');
can_ok($card, 'getSuite', 'getValue', 'getColor', 'isRed', 'isBlack');


foreach my $suite (qw(h d s c)){
   foreach my $value (2 .. 10, qw(j q k a)){
      my $specific_card = new_ok('Poker::Card'=>[$value,$suite]);

      is($specific_card->getSuite(), $suite);
      is($specific_card->getValue(), $value);
   }
}

my $red_card = new_ok('Poker::Card'=>[4,'d']);
ok($red_card->isRed());
ok(! $red_card->isBlack());

my $black_card = new_ok('Poker::Card'=>[5,'s']);
ok($black_card->isBlack());
ok(! $black_card->isRed());

done_testing();
